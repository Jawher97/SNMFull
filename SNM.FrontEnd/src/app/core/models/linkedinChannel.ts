
  // export interface LinkedInPages {
  //   pages: LinkedInPage[];
  // }

import { LinkedInInsight } from "./linkedin-insight";

  
  // export interface LinkedInPage {
  //   companyName: string;
  //   companyId: string;
  //   coverPhoto: string;
  //   brandId: string;
  // }

  export interface LinkedInChannel {
  id: string | null;
  companyName: string;
  companyUrn: string;
  companyId: string;
  coverPhoto: string;
  companyProfileUrl: string;
  insight: LinkedInInsight;
  brandId: string;

}