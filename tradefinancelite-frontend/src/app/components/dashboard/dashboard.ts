import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Lc } from '../../services/lc';
import { Auth } from '../../services/auth';
import { LcResponse } from '../../models/lc.model';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit, AfterViewInit {
  @ViewChild('statusChart') statusChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('userChart') userChartRef!: ElementRef<HTMLCanvasElement>;
  chart: Chart | null = null;
  userChartInstance: Chart | null = null;
  lcs: LcResponse[] = [];
  loading = true;
  errorMessage = '';
  role: string | null = '';
  fullName: string | null = '';
  searchTerm = '';
  statusFilter = '';
  constructor(private lcService: Lc, private authService: Auth, private router: Router) { }
  ngOnInit(): void {
    this.role = this.authService.getRole();
    this.fullName = this.authService.getFullName();
    this.loadLcs();
  }
  ngAfterViewInit(): void { }
  get filteredLcs(): LcResponse[] {
    return this.lcs.filter(lc => {
      const matchesSearch = !this.searchTerm ||
        lc.beneficiaryName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        lc.applicantName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        lc.lcNumber.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesStatus = !this.statusFilter || lc.status === this.statusFilter;
      return matchesSearch && matchesStatus;
    });
  }
  get totalCount(): number { return this.lcs.length; }
  get pendingCount(): number { return this.lcs.filter(l => l.status === 'PendingApproval').length; }
  get approvedCount(): number { return this.lcs.filter(l => l.status === 'Approved').length; }
  get rejectedCount(): number { return this.lcs.filter(l => l.status === 'Rejected').length; }
  loadLcs(): void {
    this.loading = true;
    this.lcService.getAll().subscribe({
      next: (data) => {
        this.lcs = data;
        this.loading = false;
        setTimeout(() => { this.renderChart(); this.renderUserChart(); }, 100);
      },
      error: () => {
        this.errorMessage = 'Failed to load LCs.';
        this.loading = false;
      }
    });
  }
  renderChart(): void {
    if (!this.statusChartRef) return;
    const pending = this.lcs.filter(l => l.status === 'PendingApproval').length;
    const approved = this.lcs.filter(l => l.status === 'Approved').length;
    const rejected = this.lcs.filter(l => l.status === 'Rejected').length;
    if (this.chart) {
      this.chart.destroy();
    }
    this.chart = new Chart(this.statusChartRef.nativeElement, {
      type: 'pie',
      data: {
        labels: ['Pending', 'Approved', 'Rejected'],
        datasets: [{
          data: [pending, approved, rejected],
          backgroundColor: ['#FFAB00', '#71DD37', '#FF3E1D']
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { position: 'bottom' }
        }
      }
    });
  }
  renderUserChart(): void {
    if (!this.userChartRef) return;

    const counts: { [key: string]: number } = {};
    this.lcs.forEach(lc => {
      const name = lc.createdByName || 'Unknown';
      counts[name] = (counts[name] || 0) + 1;
    });

    const labels = Object.keys(counts);
    const data = Object.values(counts);

    const ctx = this.userChartRef.nativeElement.getContext('2d');
    const gradient = ctx!.createLinearGradient(0, 0, 0, 260);
    gradient.addColorStop(0, '#8F93FF');
    gradient.addColorStop(1, '#696CFF');

    const gradientHover = ctx!.createLinearGradient(0, 0, 0, 260);
    gradientHover.addColorStop(0, '#FF7A5C');
    gradientHover.addColorStop(1, '#FF3E1D');

    if (this.userChartInstance) {
      this.userChartInstance.destroy();
    }

    this.userChartInstance = new Chart(this.userChartRef.nativeElement, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: 'LCs Created',
          data: data,
          backgroundColor: gradient,
          hoverBackgroundColor: gradientHover,
          borderRadius: 8,
          borderSkipped: false,
          barThickness: 34
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false }
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: { precision: 0 },
            grid: { color: 'rgba(105, 108, 255, 0.08)' }
          },
          x: {
            grid: { display: false }
          }
        }
      }
    });
  }
  approve(id: number): void {
    this.lcService.approve(id, 'Approved via dashboard').subscribe({
      next: () => this.loadLcs(),
      error: () => alert('Failed to approve. Are you sure you are a Checker?')
    });
  }
  reject(id: number): void {
    const reason = prompt('Enter rejection reason:');
    if (!reason) return;
    this.lcService.reject(id, reason).subscribe({
      next: () => this.loadLcs(),
      error: () => alert('Failed to reject.')
    });
  }
  goToCreate(): void {
    this.router.navigate(['/create-lc']);
  }
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  statusClass(status: string): string {
    if (status === 'Approved') return 'status-approved';
    if (status === 'Rejected') return 'status-rejected';
    return 'status-pending';
  }
}
