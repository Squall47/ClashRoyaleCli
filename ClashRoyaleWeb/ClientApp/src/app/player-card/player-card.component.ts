import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseRequest, CardDeck } from '../models/interfaces';
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'app-player-card',
  templateUrl: './player-card.component.html'
})

export class PlayerCardComponent extends BaseRequest implements OnInit {
  public mycards: CardDeck[];
  public collecteditems: MenuItem[];
  public cardtypeitems: MenuItem[];
  public cardType: number;
  @ViewChild('dt') table: any;

  constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrlStr: string) {
    super(httpClient, baseUrlStr, 'api/ClashRoyale/Cards' );
  }

  ngOnInit(): void {
    this.collecteditems = [
      {label: 'All', icon: 'glyphicon glyphicon-align-justify', command: () => {this.filterAll(); }},
      {label: 'Max', icon: 'glyphicon glyphicon-ok', command: () => {this.filterMax(); }},
      {label: 'Collected', icon: 'glyphicon glyphicon-th', command: () => {this.filterCollected(); }},
      {label: 'Missing', icon: 'glyphicon glyphicon-ban-circle', command: () => {this.filterMissing(); }},
    ];
    this.cardtypeitems = [
      {label: 'Commun', icon: 'glyphicon glyphicon-align-justify', command: () => {this.filterType(13); }},
      {label: 'Rare', icon: 'glyphicon glyphicon-ok', command: () => {this.filterType(11); }},
      {label: 'Epic', icon: 'glyphicon glyphicon-th', command: () => {this.filterType(8); }},
      {label: 'Legendaire', icon: 'glyphicon glyphicon-ban-circle', command: () => {this.filterType(5); }},
    ];
    this.refresh();
  }

  public refresh() {
    this.loading = true;
    this.http.get<CardDeck[]>(this.baseUrl).subscribe(result => {
      this.mycards = result;
      this.loading = false;
    }, error => console.error(error));
  }

  public filterType(tp: number) {
    this.cardType = tp;
    if (tp === 0) {
      this.table.filter(tp, 'maxLevel', 'gt');
    } else {
      this.table.filter(this.cardType, 'maxLevel', 'equals');
    }
  }

  public filterCollected() {
    this.table.reset();
    this.table.filter(0, 'cards', 'gte');
    this.table.filter(false, 'isMax', 'equals');
    this.filterType(this.cardType);
  }

  public filterMax() {
    this.table.reset();
    this.table.filter(true, 'isMax', 'equals');
    this.filterType(this.cardType);
  }

  public filterMissing() {
    this.table.reset();
    this.table.filter(0, 'cards', 'lt');
    this.filterType(this.cardType);
  }

  public filterAll() {
    this.table.reset();
    this.filterType(this.cardType);
  }
}
