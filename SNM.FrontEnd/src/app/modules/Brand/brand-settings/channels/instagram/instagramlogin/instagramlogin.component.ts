import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InstagramService } from 'app/core/services-api/instagram.service';

@Component({
  selector: 'instagramlogin',
  templateUrl: './instagramlogin.component.html',
  styleUrls: ['./instagramlogin.component.scss']
})
export class InstagramloginComponent implements OnInit {

  code = "";
  showWelcomeMessage: boolean = true;
  showPosts: boolean = false;
  constructor(private route: ActivatedRoute, private instagramService: InstagramService) { }

  ngOnInit() {
   
    
    this.code = this.route.snapshot.queryParams['code'];
 
   // this.instagramService.generateAccessToken(this.code);

    console.log("this.code ="+ this.route.snapshot.queryParams["code"]);
  }

}
