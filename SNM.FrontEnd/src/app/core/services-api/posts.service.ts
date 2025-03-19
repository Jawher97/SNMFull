import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, map, Observable, of, switchMap, take, tap, throwError } from 'rxjs';
import { cloneDeep } from 'lodash-es';
import { Post } from '../models/post.model';
import { environment, linkedInPaths } from 'environments/environment.development';
import { LinkedinPost } from '../models/linkedin-post';
import { LinkedInService } from './linkedin.service';
import { GenericPublishingPosts } from '../models/genericPublishingPosts.model';
import { SocialChannel } from '../models/socialChannel.model';
import { ISocialChannel } from '../types/socialChannel.types';
@Injectable({
  providedIn: 'root'
})
export class PostsService
{
    // Private
    //private _labels: BehaviorSubject<Label[] | null> = new BehaviorSubject(null);
    private _publishPost: BehaviorSubject<GenericPublishingPosts | null> = new BehaviorSubject(null);
    private _post: BehaviorSubject<Post | null> = new BehaviorSubject(null);
    private _linkedinpost: BehaviorSubject<LinkedinPost | null> = new BehaviorSubject(null);
    private _posts: BehaviorSubject<Post[] | null> = new BehaviorSubject(null);
    private _linkedinposts: BehaviorSubject<LinkedinPost[] | null> = new BehaviorSubject(null);
    private postPath;
    private getPostsUrl;
    private GetLastAgrgregator_Path;

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient, private linkedinservice: LinkedInService)
    {
        // Set the private defaults
        this.postPath=environment.postURL;
        this.getPostsUrl = linkedInPaths.linkedinAPIURL;
        this.GetLastAgrgregator_Path = environment.GetLastAgrgregator_Path;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for labels
     */
    // get labels$(): Observable<Label[]>
    // {
    //     return this._labels.asObservable();
    // }

    /**
     * Getter for posts
     */
    get posts$(): Observable<Post[]>
    {
        return this._posts.asObservable();
    }

    /**
     * Getter for post
     */
    get post$(): Observable<Post>
    {
        return this._post.asObservable();
    }
    /**
     * Getter for publishPost
     */
    get publishPost$(): Observable<GenericPublishingPosts>
    {
        return this._publishPost.asObservable();
    }


    /**
     * Getter for posts
     */
    get linkedinposts$(): Observable<LinkedinPost[]>
    {
        return this._linkedinposts.asObservable();
    }

    /**
     * Getter for post
     */
    get linkedinpost$(): Observable<LinkedinPost>
    {
        return this._linkedinpost.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get labels
     */
    // getLabels(): Observable<Label[]>
    // {
    //     return this._httpClient.get<Label[]>('api/apps/posts/labels').pipe(
    //         tap((response: Label[]) => {
    //             this._labels.next(response);
    //         })
    //     );
    // }

    /**
     * Add label
     *
     * @param title
     */
    // addLabel(title: string): Observable<Label[]>
    // {
    //     return this._httpClient.post<Label[]>('api/apps/posts/labels', {title}).pipe(
    //         tap((labels) => {

    //             // Update the labels
    //             this._labels.next(labels);
    //         })
    //     );
    // }

    /**
     * Update label
     *
     * @param label
     */
    // updateLabel(label: Label): Observable<Label[]>
    // {
    //     return this._httpClient.patch<Label[]>('api/apps/posts/labels', {label}).pipe(
    //         tap((labels) => {

    //             // Update the posts
    //             this.getPosts().subscribe();

    //             // Update the labels
    //             this._labels.next(labels);
    //         })
    //     );
    // }

    /**
     * Delete a label
     *
     * @param id
     */
    // deleteLabel(id: string): Observable<Label[]>
    // {
    //     return this._httpClient.delete<Label[]>('api/apps/posts/labels', {params: {id}}).pipe(
    //         tap((labels) => {

    //             // Update the posts
    //             this.getPosts().subscribe();

    //             // Update the labels
    //             this._labels.next(labels);
    //         })
    //     );
    // }

    /**
     * Get posts
     */
    getPosts(): Observable<Post[]>
    {
        return this._httpClient.get<Post[]>(this.postPath+"GetAll").pipe(
            tap((response: Post[]) => {
                this._posts.next(response);
            })
        );
    }
    getAllPosts(channels:ISocialChannel[]): Observable<any>
    {
        return this._httpClient.post<any[]>(this.GetLastAgrgregator_Path  +"/GetLastPost",channels).pipe(
            tap((response: any) => {

                this._posts.next(response.data);
            })
        );
    }

    
    /**
     * GetLinkedinLatestPost
     */
    getLatestPost(): Observable<LinkedinPost> {
        return this.linkedinservice.getLatestPost("urn:li:organization:92781486");
      }

    /**
     * Get LinkedIn Posts
     */
    getLinkedInPosts(): Observable<LinkedinPost[]>
    {
        const accesstoken = localStorage.getItem('LinkedIn access_token');
        const memberUrn = localStorage.getItem('org_urn');

        const params = new HttpParams()
        .set('accessToken', accesstoken)
        .set('memberUrn', memberUrn);

        return this._httpClient.get<LinkedinPost[]>(this.getPostsUrl+"GetLinkedinPosts", {params: params}).pipe(
            tap((response: LinkedinPost[]) => {
                this._linkedinposts.next(response);
            })
        );
    }

    /**
     * Get post by id
     */
    getPostById(id: string): Observable<Post>
    {
        return this._posts.pipe(
            take(1),
            map((posts) => {

                // Find within the folders and files
                const post = posts.find(value => value.id === id) || null;

                // Update the post
                this._post.next(post);

                // Return the post
                return post;
            }),
            switchMap((post) => {

                if ( !post )
                {
                    return throwError('Could not found the post with id of ' + id + '!');
                }

                return of(post);
            })
        );
    }

    /**
     * Add task to the given post
     *
     * @param post
     * @param task
     */
    addTask(post: Post, task: string): Observable<Post>
    {
        return this._httpClient.post<Post>('api/apps/posts/tasks', {
            post,
            task
        }).pipe(switchMap(() => this.getPosts().pipe(
            switchMap(() => this.getPostById(post.id))
        )));
    }

    /**
     * Create post
     *
     * @param post
     */
    createPost(post: any): Observable<any>
    {
        return this._httpClient.post<any>(this.postPath+'CreatePost', post).pipe(
            switchMap(response => this.getPosts().pipe(
                switchMap(() => this.getPostById(response.data).pipe(
                    map(() => response)
                ))
            )));
    }

    /**
     * Update the post
     *
     * @param post
     */
    updatePost(post: Post): Observable<Post>
    {
        // Clone the post to prevent accidental reference based updates
        const updatedPost = cloneDeep(post) as any;

        // Before sending the post to the server, handle the labels
        if ( updatedPost.labels.length )
        {
            updatedPost.labels = updatedPost.labels.map(label => label.id);
        }

        return this._httpClient.patch<Post>('api/apps/posts', {updatedPost}).pipe(
            tap((response) => {

                // Update the posts
                this.getPosts().subscribe();
            })
        );
    }

    /**
     * Delete the post
     *
     * @param post
     */
    deletePost(post: Post): Observable<boolean>
    {
        return this._httpClient.delete<boolean>('api/apps/posts', {params: {id: post.id}}).pipe(
            map((isDeleted: boolean) => {

                // Update the posts
                this.getPosts().subscribe();

                // Return the deleted status
                return isDeleted;
            })
        );
    }
}
