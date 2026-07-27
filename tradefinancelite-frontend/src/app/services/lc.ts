import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateLcRequest, LcResponse } from '../models/lc.model';

@Injectable({
  providedIn: 'root'
})
export class Lc {
  private baseUrl = 'http://localhost:5259/api/Lc';

  constructor(private http: HttpClient) { }

  getAll(): Observable<LcResponse[]> {
    return this.http.get<LcResponse[]>(this.baseUrl);
  }

  create(data: CreateLcRequest): Observable<LcResponse> {
    return this.http.post<LcResponse>(this.baseUrl, data);
  }

  approve(id: number, remarks: string): Observable<LcResponse> {
    return this.http.post<LcResponse>(`${this.baseUrl}/${id}/approve`, { remarksOrReason: remarks });
  }

  reject(id: number, remarks: string): Observable<LcResponse> {
    return this.http.post<LcResponse>(`${this.baseUrl}/${id}/reject`, { remarksOrReason: remarks });
  }
}
