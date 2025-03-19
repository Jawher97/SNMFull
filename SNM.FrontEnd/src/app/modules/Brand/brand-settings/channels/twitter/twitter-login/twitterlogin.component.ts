import { Component } from '@angular/core';
import { TwitterService } from 'app/core/services-api/twitter.service';

@Component({
  selector: 'twitter-login',
  templateUrl: './twitterlogin.component.html',
  styleUrls: ['./twitterlogin.component.scss']
})
export class TwitterloginComponent {

  private twitterCredentials = {
    clientId: "aFF4djZyV2NsRUtKaURuYTFneDA6MTpjaQ",
    redirectUrl: "https://localhost:4200/twittercallback",
    scope: "tweet.read%20tweet.write%20users.read%20follows.read%20follows.write%20dm.read%20dm.write%20offline.access"
  };

  private authUrl = `https://twitter.com/i/oauth2/authorize?response_type=code&client_id=aFF4djZyV2NsRUtKaURuYTFneDA6MTpjaQ&redirect_uri=https://localhost:4200/twittercallback&scope=tweet.read%20tweet.write%20users.read%20follows.read%20follows.write%20dm.read%20dm.write%20offline.access&state=state&code_challenge=nothing&code_challenge_method=plain`;

  connected: boolean;

  constructor(private twitterService: TwitterService) {
          // Vérifiez si le token LinkedIn est présent dans le localStorage
          const twittertoken = localStorage.getItem('Twitter access_token');
  
          // Mettez à jour la variable connected en fonction de la présence du token
          this.connected = !!twittertoken;
  }
  
  login() {
    this.twitterService.login();  
    

    this.connected = true

  }

  logOut(): void {
    localStorage.removeItem('Twitter access_token');
    localStorage.removeItem('Twitter refresh_token');
    this.connected=false
  }
}
