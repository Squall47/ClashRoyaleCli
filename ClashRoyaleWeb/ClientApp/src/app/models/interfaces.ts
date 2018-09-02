import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export abstract class BaseRequest {
  public baseUrl: string;
  public http: HttpClient;
  public loading: boolean;

  constructor(httpClient: HttpClient, baseUrlStr: string, relativepath: string) {
    this.baseUrl = baseUrlStr + relativepath;
    this.http = httpClient;
  }
}

export interface Tournament {
  start: string;
  places: number;
  status: string;
  name: string;
  maxCapacity: number;
}

export interface CardDeck extends CardPlayer {
  cards: number;
  cardType: string;
  isMax: boolean;
}

export interface CardPlayer extends CardBase {
  level: number;
  count: number;
}

export interface CardBase {
  name: string;
  id: number;
  maxLevel: number;
  iconUrls: CardIconUrls;
  imgMarkDown: string;
  icon: string;
  cardType: string;
}

export interface CardIconUrls {
  medium: string;
}

export interface TournamentItem {
  start: string;
  tag: string;
  type: string;
  status: string;
  creatorTag: string;
  name: string;
  description: string;
  capacity: number;
  maxCapacity: number;
  places: number;
  preparationDuration: number;
  duration: number;
  createdTime: Date;
}
