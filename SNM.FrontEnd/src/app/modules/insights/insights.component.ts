import { Component } from '@angular/core';

@Component({
  selector: 'app-insights',
  templateUrl: './insights.component.html',
  styleUrls: ['./insights.component.scss']
})
export class InsightsComponent {

  selectedAccount: string;
  facebookStats: any[] = [];
  linkedinStats: any[] = [];
  instagramStats: any[] = [];
  twitterStats: any[] = [];

  
  onAccountSelected() {
    // Mettez en œuvre la logique pour charger les statistiques en fonction du compte sélectionné
    if (this.selectedAccount === 'facebook') {
      this.facebookStats = [
        { label: 'Followers', value: '1000' },
        { label: 'Impressions', value: '5000' },
        { label: 'Nombre total de commentaires', value: '200' }
      ];
    } else if (this.selectedAccount === 'linkedin') {
      this.linkedinStats = [
        { label: 'Followers', value: '500' },
        { label: 'Impressions', value: '3000' },
        { label: 'Nombre total de commentaires', value: '100' }
      ];
    } else if (this.selectedAccount === 'instagram') {
      this.instagramStats = [
        { label: 'Followers', value: '2000' },
        { label: 'Impressions', value: '8000' },
        { label: 'Nombre total de commentaires', value: '300' }
      ];
    } else if (this.selectedAccount === 'twitter') {
      this.twitterStats = [
        { label: 'Followers', value: '1500' },
        { label: 'Impressions', value: '4000' },
        { label: 'Nombre total de commentaires', value: '150' }
      ];
    }
  }

}
