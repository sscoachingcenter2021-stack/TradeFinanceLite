export interface CreateLcRequest {
  applicantName: string;
  beneficiaryName: string;
  amount: number;
  currency: string;
  issueDate: string;
  expiryDate: string;
  terms: string;
}

export interface LcResponse {
  id: number;
  lcNumber: string;
  applicantName: string;
  beneficiaryName: string;
  amount: number;
  currency: string;
  issueDate: string;
  expiryDate: string;
  terms: string;
  status: string;
  createdByName: string;
  approvedByName: string | null;
  isFlagged: boolean;
  screeningScore: number;
}
