import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TwitterService } from 'app/core/services-api/twitter.service';

@Component({
  selector: 'app-twittercallback',
  templateUrl: './twittercallback.component.html',
  styleUrls: ['./twittercallback.component.scss']
})
export class TwittercallbackComponent {

  code = "";
  
  constructor(private route: ActivatedRoute, private twitterService: TwitterService) {}
  
  ngOnInit(): void {
    this.code=this.route.snapshot.queryParams["code"];
    window.opener.postMessage("twitterCode:"+this.code, "*");
    console.log("aaaaaa")
    //  this.twitterService.generateAccessToken(this.code)
    }
  }

