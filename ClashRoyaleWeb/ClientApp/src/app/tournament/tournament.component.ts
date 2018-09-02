import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseRequest, TournamentItem } from '../models/interfaces';

@Component({
  selector: 'app-tournament',
  templateUrl: './tournament.component.html'
})
export class TournamentComponent extends BaseRequest {
  public tournaments: TournamentItem[];

  constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrlStr: string) {
    super(httpClient, baseUrlStr, 'api/ClashRoyale/Tournois');
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
