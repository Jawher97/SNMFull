export const environment = {
    production: false,
    publishingAgrgregatorURL: 'https://localhost:44311/api/v1/Publishing/',
    GetLastAgrgregator_Path:'https://localhost:44311/api/v1/GetLastPosts',
    Comment_Path:'https://localhost:44311/api/v1/Comment',
    Reaction_Path:'https://localhost:44311/api/v1/Reactions',
    brandURL: 'https://localhost:7004/api/v1/Brand/',
    channelURL: 'https://localhost:7004/api/v1/Channel/',
    channelTypeURL: 'https://localhost:7004/api/v1/ChannelType/',
    postURL: 'https://localhost:44303/api/v1/Post/',
    uplodeURL:'https://localhost:44303/api/v1/Brand',
    apiUrl: 'https://localhost:4200',
    getPostfacebook:"https://localhost:44350/api/v1/FacebookPostAPI/GetLatestPostFieldsByPageId",
    GetFacebookSummary:"https://localhost:44350/api/v1/FacebookPostAPI",
    getCommentFacebook:"https://localhost:44350/api/v1/FacebookPostAPI/GetLastPostCommentsById",
    latestInstagramPost:"https://localhost:44310/api/v1/InstagramPostAPI/GetInstagramMedia",
    InstagramSummary:"https://localhost:44310/api/v1/InstagramPostAPI/GetInstagramSummary",
    latestInstagramComment:"https://localhost:44310/api/v1/InstagramPostAPI/comments",
    latestLinkedinPost:"https://localhost:44383/apiLinkedIn/v1/LinkedInAPI/GetLatestPost",
    latestLinkedinComment:"https://localhost:44383/apiLinkedIn/v1/LinkedInAPI/RetrieveComments",
    /**Facebook */
    facebookAPIURL: 'https://localhost:44303/api/v1/FacebookAPI/',
    facebookChannelIURL: 'https://localhost:44303/api/v1/FacebookChannel/',
    facebookPostURL: 'https://localhost:44303/api/v1/FacebookPost/',
    facebookPostAPIURL: 'https://localhost:44303/api/v1/FacebookPostAPI/',
    facebookAppId: '526589669666269',//'1668572883610911',//'1290057835057119',//'776558987451725',
    /** Instagram */
    InstagramPostAPIURL: 'https://localhost:44310/api/v1/InstagramPostAPI/PublishPostToInstagram',
    // instagramClientId : '261825306411994',
    // instagramClientSecret : '42e2bc47db7429002a2d89b74f51036e',
    // instagramRedirectUri : 'https://localhost:4200/callback/',
    // instagramScope : 'user_profile,user_media',
    /** LinkedIn */
    
    // linkedInScope: "openid%20profile%20email",
    // linkedInAuthAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAuth/',
    // linkedinOAuthURL: 'https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=',
    // linkedinAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAPI/',
    /** Twitter */
    twitterAuthAPIURL: 'https://localhost:44358/apitwitter/v1/TwitterOAuth/',
    twitterPath: 'https://localhost:44358/apitwitter/v1/twitter/',
    twitterOAuthURL: 'https://api.twitter.com/oauth/authorize?oauth_token=',
    clientId: 'aFF4djZyV2NsRUtKaURuYTFneDA6MTpjaQ',
    twitterv2AuthURL: 'https://twitter.com/i/oauth2/authorize?response_type=code&client_id=aFF4djZyV2NsRUtKaURuYTFneDA6MTpjaQ&redirect_uri=https://localhost:4200/twittercallback&scope=tweet.read%20tweet.write%20users.read%20follows.read%20follows.write%20dm.read%20dm.write%20offline.access&state=state&code_challenge=nothing&code_challenge_method=plain'
};
// export const instagramCredentials={
//     instagramClientId : '261825306411994',
//     instagramClientSecret : '8d6fcafe9532735cf19014b790ae51fa',
//     instagramRedirectUri : 'https://localhost:4200/callback/',
//     instagramScope : 'user_profile,user_media',
// }
export const instagramCredentials={
    oauthUrl:`https://api.instagram.com/oauth/authorize?client_id=558877136436062&redirect_uri=https://localhost:4200/callback&scope=user_profile,user_media,instagram_graph_user_profile,instagram_graph_user_media,user_link&response_type=code`,
    instagramClientId : '558877136436062',
    instagramClientSecret : '3b5009771ae1305c2e81d0e52c6fff8e',
    instagramRedirectUri : 'https://localhost:4200/callback/',
 instagramScope : 'user_profile,user_media,instagram_graph_user_profile,instagram_graph_user_media',
    // instagramScope : 'user_profile,user_media',
}
export const facebookCredentials={
    facebookClientId : '526589669666269',
    // instagramClientSecret : '3b5009771ae1305c2e81d0e52c6fff8e',
    facebookRedirectUri : 'https://localhost:4200/facebookcallback/',
    // instagramScope : 'user_profile,user_media,instagram_graph_user_profile,instagram_graph_user_media',
    facebookScope : 'public_profile,email',
}
export const linkedInCredentials={
    
    linkedInClientId: "77vkkcv6n3ct4x",//"787lzflx5f7688",//"77vkkcv6n3ct4x",
    linkedInRedirectUrl: "https://localhost:4200/callback",
    // linkedInScope: "openid%20profile%20email",
    linkedInScope:"r_liteprofile%20r_emailaddress%20w_member_social%20rw_organization_admin%20r_organization_social%20w_organization_social%20r_organization_admin",
    }
export const facebookPaths={
    oauthUrl:`https://www.facebook.com/v17.0/dialog/oauth?client_id=261825306411994&redirect_uri=https://localhost:4200/facebookcallback&scope=public_profile,email,instagram_basic,pages_show_list,user_link,pages_read_engagement,pages_read_user_content,read_insights,user_videos,ads_management,ads_read,pages_manage_engagement,pages_manage_posts,pages_manage_metadata,pages_manage_ads&response_type=code`,
    facebookAPIURL: 'https://localhost:44350/api/v1/FacebookAPI/',
    facebookChannelIURL: 'https://localhost:44303/api/v1/FacebookChannel/',
    facebookPostURL: 'https://localhost:44303/api/v1/FacebookPost/',
    facebookPostAPIURL: 'https://localhost:44303/api/v1/FacebookPostAPI/'
}
export const facebookGroupsPaths={
    oauthUrl:`https://www.facebook.com/v17.0/dialog/oauth?client_id=261825306411994&redirect_uri=https://localhost:4200/facebookForGroupscallback&scope=public_profile,email,instagram_basic,groups_show_list,user_link,pages_read_engagement,pages_read_user_content,read_insights,user_videos,ads_management,ads_read,groups_access_member_info,user_posts,pages_manage_posts&response_type=code`,
    facebookAPIURL: 'https://localhost:44350/api/v1/FacebookAPI/',
    facebookChannelIURL: 'https://localhost:44303/api/v1/FacebookChannel/',
    facebookPostURL: 'https://localhost:44303/api/v1/FacebookPost/',
    facebookPostAPIURL: 'https://localhost:44303/api/v1/FacebookPostAPI/'
}
export const instagramPaths={
   // oauthUrl:`https://api.instagram.com/oauth/authorize?client_id=261825306411994&redirect_uri=https://localhost:4200/instagramcallback&scope=user_profile,user_media,instagram_graph_user_profile,instagram_graph_user_media,user_link,user_photos,email&response_type=code`,
    oauthUrl:`https://www.facebook.com/v17.0/dialog/oauth?client_id=261825306411994&redirect_uri=https://localhost:4200/instagramcallback&scope=public_profile,email,instagram_basic,pages_show_list,user_link,instagram_manage_comments,pages_read_engagement&response_type=code`,

    linkedInScope: "openid%20profile%20email",
    linkedInAuthAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAuth/',
    linkedinOAuthURL: 'https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=',
    //linkedinAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAPI/',
    instagramAPIURL: 'https://localhost:44310/api/v1/InstagramAPI/GetInstagramAccessToken',

}
export const linkedInPaths={
    oauthUrl:`https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=true&client_id=782e04t5j1j892&redirect_uri=https://localhost:4200/callback&scope=profile%20email%20w_member_social%20rw_organization_admin%20r_organization_social%20w_organization_social%20r_organization_admin%20r_basicprofile%20r_1st_connections_size%20rw_ads%20r_ads%20r_ads_reporting%20openid%20w_member_social`,
    linkedInScope: "openid%20profile%20email",
    linkedInAuthAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAuth/',
    linkedinOAuthURL: 'https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=',
    linkedinAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAPI/',
}
export const twitterPaths={


    oauthUrl:'https://twitter.com/i/oauth2/authorize?response_type=code&client_id=aFF4djZyV2NsRUtKaURuYTFneDA6MTpjaQ&redirect_uri=https://localhost:4200/twittercallback&scope=tweet.read%20tweet.write%20users.read%20follows.read%20follows.write%20dm.read%20dm.write%20offline.access&state=state&code_challenge=nothing&code_challenge_method=plain',
    linkedInScope: "openid%20profile%20email",
    linkedInAuthAPIURL: 'https://localhost:44358/apitwitter/v1/TwitterOAuth/',
    linkedinOAuthURL: 'https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=',
    linkedinAPIURL: 'https://localhost:44383/apiLinkedIn/v1/LinkedInAPI/',
}

//  private linkedInCredentials = {
//     clientId: "77vkkcv6n3ct4x",//"787lzflx5f7688",//"77vkkcv6n3ct4x",
//     redirectUrl: "https://localhost:4200/callback",
//      scope: "r_liteprofile%20r_emailaddress%20w_member_social%20rw_organization_admin%20r_organization_social%20w_organization_social%20r_organization_admin"
//     // scope: "openid%20profile%20email%20r_basicprofile"//%20openid%20profile%20email%20w_member_social%20rw_organization_admin%20r_compliance"
//     // scope: "openid%20profile%20email"//%20openid%20profile%20email%20w_member_social%20rw_organization_admin%20r_compliance"
//   };