# Music solution

This solution is built using .Net Core 3.1 and VS 2019 Community. I have started and built it on a Windows machine and hopefully there shouldn't be any issues if you run it on Mac.

You can simply download or clone the code and run it in Development mode with IIS Express.

Once you run the project, you will have a single endpoint available at http://localhost:51000/api/search/{artistName}

You can view following sample responses mentioned below either in a browser or in Postman.

- Sample GET response for a single artist: http://localhost:51000/api/search/%22Hayley%20Williams%22
- Sample GET response for multiple artists: http://localhost:51000/api/search/hayley

The API response looks like this:

```
{
   results: [],
   message: ""
}
```

## Solution Design

The project looks small at first but there are some discussion points we can chat about.

To begin with, we would have separate projects (microservices) for Artists and Releases if it was our own implementation.
Thought of creating a separate project for MusicBrainz services but have opted to include it in Search project for this exercise.

One aspect of given requirements was to return the releases in place of artists collection when there is one artist in search results.

The current implementation is not doing exactly that. At the moment we just include the releases data in artists response when search results have a single artist only.

To maintain consistency, I have added a releases property within an artist model which is always present. It helps to maintain consistency for the response type on a single endpoint.

So for a normal search response, the format of collection looks like this: 

```
[
  {
    /* artist info here */
    releases: null
  }
]
```

When search results have a single artist response:

```
[
  {
    /* artist info here */
    releases: [ /* releases info here */ ]
  }
]
```

In practice I would discuss whether to include the releases data in a search result at all if it is not really needed immediately by the client. I would prefer a separate endpoint where client can fetch the release data for artist directly without using a search engine. We can discuss this further and see if there is a better way to implement it.

I have added Polly in case one of the services fails and we can still return a default or degraded response to the original request.

Overall structure of the Search project should be self explanatory. It is using one Search controller and a related service. Search service uses Artists and Releases services to get results from MusicBrainz. 

## Unit Tests

The test project is setup with xUnit. Have added tests for SearchService and ReleasesService. Similar tests can be added for ArtistsService later.

## To Do
- It is not production ready. Currently only Development environment is setup.
- Have not included Docker support yet and it is not connected to Azure DevOps.

## Further considerations

- Pagination is not implemented. Currently it fetches first 10 records for Artists or Releases.
- The logger should use consistent message format along with data that should be logged.
- Currently there is no transformation of data we receive from MusicBrainz.
- HTTPS is not enabled at the moment.
- Currently only basic properties are available on Artist and Release models. We can add other properties as well.
- Versioning support is not enabled.
- There is no authentication setup.
- Swagger is not available yet.
- I would think part of the music data doesn't change too often so can consider caching of results.
- As with any API development, performance and security testing is required once we have a production ready API.

## PS

So API Management can be configured to return the responses from MusicBrainz if we are not changing the properties. In this case I suppose we don't need our implementation on top of MusicBrainz.
