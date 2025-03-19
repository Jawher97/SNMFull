import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { InstagramService } from 'app/core/services-api/instagram.service';

@Component({
  selector: 'instagram',
  templateUrl: './instagram.component.html',
  styleUrls: ['./instagram.component.scss']
})
export class InstagramComponent {
  // clientId = '261825306411994';
  // clientSecret = '42e2bc47db7429002a2d89b74f51036e';
  // redirectUri = 'https://localhost:4200/callback/';
  // scope = 'user_profile,user_media,public_content';
  code: string;
  accessToken: string;
  userId: string;
  // private url = `https://api.instagram.com/oauth/authorize?client_id=${this.clientId}&redirect_uri=${this.redirectUri}&response_type=code&scope=${this.scope}`;
  instagramservice: any;
  connected: boolean = false;

  constructor(private http: HttpClient, private route: ActivatedRoute, private instagramService: InstagramService) { // Vérifiez si le token LinkedIn est présent dans le localStorage

    const accessToken = localStorage.getItem('instagram_access_token');



    // Mettez à jour la variable connected en fonction de la présence du token

    this.connected = !!accessToken;

  }



  login() {
    console.log('login to get instagram_access_token')
    this.instagramService.login();
    this.connected = true;

  }
  logOut(): void {

    localStorage.removeItem('instagram_access_token');

    this.connected = false

  }

}

