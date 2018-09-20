import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseRequest, TournamentItem } from '../models/interfaces';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html'
})
export class TournamentComponent extends BaseRequest implements OnInit {
  public tournaments: TournamentItem[];
  capacity: SelectItem[];

  constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrlStr: string) {
    super(httpClient, baseUrlStr, 'api/ClashRoyale/Tournois');
  }

  ngOnInit(): void {
    this.capacity = [
      { label: 'Max', value: null },
      { label: '50', value: 50 },
      { label: '100', value: 100 },
      { label: '1000', value: 1000 }
    ];
    this.refresh();
  }

  public refresh() {
    this.loading = true;
    this.http.get<TournamentItem[]>(this.baseUrl).subscribe(result => {
      this.tournaments = result;
      this.loading = false;
    }, error => console.error(error));
  }
}
