# SugarTown 
## Introduction

**Welcome to SugarTown!**

This is a blog engine written in [NancyFX][1] that allows you to write blog posts and then exposes the contents via an API to allow you to incorporate a blog into an existing website. 

**Why SugarTown?**

NancyFX gets its name from the Ruby web framework [Sinatra][2] (Nancy was the daughter of Frank) so I thought I'd use this type of name formatting to select a name for this project.  I thought I would choose one of Nancy Sinatra's best known recordings* and out of the options, I chose the first (you might think I should have chosen the second) plus SugarTown sounds good for a piece of software!

**Basic Usage**

Visit mydomain.com/SugarTown/posts/page/1 to add, view, update and delete blog posts ![screenshot][4]

In your website use the API to access the blog posts

    public Post GetPost(string title, string url)
    {
        var client = new RestClient(url);
    
        var request = new RestRequest("SugarTown/Posts/" + title, Method.GET);
               
        request.AddHeader("Accept", "application/json");

        var model = client.Execute<Post>(request).Data;

        return model;
    }

**Demos**

Please check out the demo projects to see how to integrate SugarTown into your website ![screenshot][3]

  [1]: http://nancyfx.org/
  [2]: http://www.sinatrarb.com/
  [3]: http://i.imgur.com/h7e1g.jpg
  [4]: http://i.imgur.com/S8XtE.jpg
  <sub>* Sugar Town & Something Stupid</sub>