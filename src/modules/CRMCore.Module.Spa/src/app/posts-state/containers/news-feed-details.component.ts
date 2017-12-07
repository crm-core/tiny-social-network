import { Component, OnInit } from '@angular/core';
import { Post } from '../models/post.model';

import { Location } from '@angular/common';

@Component({
    templateUrl: './news-feed-details.component.html'
})
export class NewsFeedDetailsComponent implements OnInit {

  post: Post  ;
  constructor(private _location: Location) {
  }

  ngOnInit(): void {
    this.post = {
      "id": "6082cd6a-5d44-49c0-8d2d-bf4f4fd44ccf",
      "title": "Cultivated who resolution connection motionless did occasional.",
      "description": "Le Journey promise if it colonel. Can all mirth abode nor hills added. Them men does for body pure.",
      "createdDate": new Date()
    };
  }

  onAddComment($event) {
    console.log('Add comment:' + $event.content);
  }

  onBack() {
    this._location.back();
  }
}
