import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Lc } from '../../services/lc';
import { Auth } from '../../services/auth';
import { LcResponse } from '../../models/lc.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  lcs: LcResponse[] = [];
  loading = true;
  errorMessage = '';
  role: string | null = '';
  fullName: string | null = '';

  constructor(private lcService: Lc, private authService: Auth, private router: Router) { }

  ngOnInit(): void {
    this.role = this.authService.getRole();
    this.fullName = this.authService.getFullName();
    this.loadLcs();
  }

  loadLcs(): void {
    this.loading = true;
    this.lcService.getAll().subscribe({
      next: (data) => {
        this.lcs = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load LCs.';
        this.loading = false;
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
    this.lcService.reject(id, 'Rejected via dashboard').subscribe({
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
