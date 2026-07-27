import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Lc } from '../../services/lc';

@Component({
  selector: 'app-create-lc',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-lc.html',
  styleUrl: './create-lc.css'
})
export class CreateLc {
  applicantName = '';
  beneficiaryName = '';
  amount: number | null = null;
  currency = 'USD';
  issueDate = '';
  expiryDate = '';
  terms = '';
  errorMessage = '';

  constructor(private lcService: Lc, private router: Router) { }

  onSubmit(): void {
    this.errorMessage = '';
    this.lcService.create({
      applicantName: this.applicantName,
      beneficiaryName: this.beneficiaryName,
      amount: Number(this.amount),
      currency: this.currency,
      issueDate: this.issueDate,
      expiryDate: this.expiryDate,
      terms: this.terms
    }).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        this.errorMessage = err.error?.message || err.error || 'Failed to create LC. Are you sure you are a Maker?';
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/dashboard']);
  }
}
