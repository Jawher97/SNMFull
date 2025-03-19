import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, of, switchMap, take, tap, throwError } from 'rxjs';
import { cloneDeep } from 'lodash-es';

import { environment } from 'environments/environment.development';
import { InstagramPost } from '../models/instagram-post.model';
@Injectable({
  providedIn: 'root'
})
export class InstagramPostsService
{
    // Private
    //private _labels: BehaviorSubject<Label[] | null> = new BehaviorSubject(null);
    private _post: BehaviorSubject<InstagramPost | null> = new BehaviorSubject(null);
    private _posts: BehaviorSubject<InstagramPost[] | null> = new BehaviorSubject(null);
    private postPath;

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient)
    {
        // Set the private defaults
        this.postPath=environment.postURL;
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
    get posts$(): Observable<InstagramPost[]>
    {
        return this._posts.asObservable();
    }

    /**
     * Getter for post
     */
    get post$(): Observable<InstagramPost>
    {
        return this._post.asObservable();
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
    getPosts(): Observable<InstagramPost[]>
    {
        return this._httpClient.get<InstagramPost[]>(this.postPath+"GetAll").pipe(
            tap((response: InstagramPost[]) => {
                this._posts.next(response);
            })
        );
    }

    /**
     * Get post by id
     */
    getPostById(id: string): Observable<InstagramPost>
    {
        return this._posts.pipe(
            take(1),
            map((posts) => {

                // Find within the folders and files
                const post = posts.find(value => value.Id === id) || null;

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
    addTask(post: InstagramPost, task: string): Observable<InstagramPost>
    {
        return this._httpClient.post<InstagramPost>('api/apps/posts/tasks', {
            post,
            task
        }).pipe(switchMap(() => this.getPosts().pipe(
            switchMap(() => this.getPostById(post.Id))
        )));
    }

    /**
     * Create post
     *
     * @param post
     */
    createPost(post: any): Observable<InstagramPost>
    {

        return this._httpClient.post<InstagramPost>(this.postPath+'CreatePost', post).pipe(
            switchMap(response => this.getPosts().pipe(
                switchMap(() => this.getPostById(response.Id).pipe(
                    map(() => response)
                ))
            )));
    }

    /**
     * Update the post
     *
     * @param post
     */
    updatePost(post: InstagramPost): Observable<InstagramPost>
    {
        // Clone the post to prevent accidental reference based updates
        const updatedPost = cloneDeep(post) as any;

        // Before sending the post to the server, handle the labels
        if ( updatedPost.labels.length )
        {
            updatedPost.labels = updatedPost.labels.map(label => label.id);
        }

        return this._httpClient.patch<InstagramPost>('api/apps/posts', {updatedPost}).pipe(
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
    deletePost(post: InstagramPost): Observable<boolean>
    {
        return this._httpClient.delete<boolean>('api/apps/posts', {params: {id: post.Id}}).pipe(
            map((isDeleted: boolean) => {

                // Update the posts
                this.getPosts().subscribe();

                // Return the deleted status
                return isDeleted;
            })
        );
    }
}
