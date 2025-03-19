import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FacebookService } from 'app/core/services-api';

@Component({
  selector: 'app-facebook-login',
  templateUrl: './facebook-login.component.html',
  styleUrls: ['./facebook-login.component.scss']
})
export class FacebookLoginComponent {
  code = "";
  showWelcomeMessage: boolean = true;
  showPosts: boolean = false;
  constructor(private route: ActivatedRoute, private facebookService: FacebookService) { }
  ngOnInit() {
   
  
    this.code = this.route.snapshot.queryParams['code'];
 
    // this.facebookService.generateAccessToken(this.code);

    console.log("this.code ="+ this.route.snapshot.queryParams["code"]);
  }
  click(){
  
    this.code = this.route.snapshot.queryParams['code'];
 
    // this.facebookService.generateAccessToken(this.code);

    console.log("this.code ="+ this.route.snapshot.queryParams["code"]);
  }
}
