﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace CoreQiita
{
    public class Items
    {
        internal HttpClient Client { get; set; }

        public ItemData[] GetAuthUserItem(int page = 1,int per_page = 5)
        {
            var async = GetAuthUserItemAsync(page, per_page);
            async.Wait();
            return async.Result;
        }

        public async Task<ItemData[]> GetAuthUserItemAsync(int page = 1,int per_page = 5)
        {
            var url = $"api/v2/authenticated_user/items?page={page}&per_page={per_page}";
            return await GetItemAsync(url);
        }

        public ItemData[] GetUserItem(string user_id,int page = 1, int per_page = 5)
        {
            var async = GetUserItemAsync(user_id, page, per_page);
            async.Wait();
            return async.Result;
        }

        public async Task<ItemData[]> GetUserItemAsync(string user_id,int page = 1,int per_page = 5)
        {
            var url = $"api/v2/users/{user_id}/items?page={page}&per_page={per_page}";
            return await GetItemAsync(url);
        }

        private async Task<ItemData[]> GetItemAsync(string url)
        {
            var message = await Client.GetAsync(url);
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<ItemData[]>(response);
            return Result;
        }

        public bool Create(string title, Tags tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var t = new Tags[] { tags };
            return Create(title, t, body, gist, isprivate, tweet);
        }

        public bool Create(string title, Tags[] tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var async = CreateAsync(title, tags, body, gist, isprivate, tweet);
            async.Wait();
            return async.Result;
        }

        public async Task<bool> CreateAsync(string title, Tags tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var t = new Tags[] { tags };
            return await CreateAsync(title, t, body, gist, isprivate, tweet);
        }

        public async Task<bool> CreateAsync(string title, Tags[] tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var data = new PostItemData()
            {
                Body = body,
                Gist = gist,
                Private = isprivate,
                Tags = tags,
                Title = title,
                Tweet = tweet
            };
            var jsondata = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsondata, Encoding.UTF8, ContentType.Json);
            var response = await Client.PostAsync("api/v2/items", content);
            return (int)response.StatusCode == 201;
        }
    }

    [JsonObject]
    internal class PostItemData
    {
        [JsonProperty("body")]
        internal string Body { get; set; }

        [JsonProperty("gist")]
        internal bool Gist { get; set; }

        [JsonProperty("private")]
        internal bool Private { get; set; }

        [JsonProperty("tags")]
        internal Tags[] Tags { get; set; }

        [JsonProperty("title")]
        internal string Title { get; set; }

        [JsonProperty("tweet")]
        internal bool Tweet { get; set; }
    }

    [JsonObject]
    public class ItemData
    {
        [JsonProperty("rendered_body")]
        public string RenderedBody { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("created_at")]
        internal string _Date { get; set; }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                if (_date != DateTime.MinValue) return _date;
                _date = DateTime.Parse(_Date);
                return _date;
            }
        }

        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("likes_count")]
        public int LikesCount { get; set; }

        [JsonProperty("private")]
        public bool isPrivate { get; set; }

        [JsonProperty("tags")]
        public Tags[] Tags { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("updated_at")]
        internal string _UpdateDate { get; set; }

        private DateTime _updatedate;
        public DateTime UpdateDate
        {
            get
            {
                if (_updatedate != DateTime.MinValue) return _updatedate;
                _updatedate = DateTime.Parse(_UpdateDate);
                return _updatedate;
            }
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("user")]
        public UserJson User { get; set; }

        [JsonProperty("page_views_count")]
        public int? Views { get; set; }
    }

    [JsonObject]
    public class Group
    {
        [JsonProperty("created_at")]
        internal string _Date { get; set; }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                if (_date != DateTime.MinValue) return _date;
                _date = DateTime.Parse(_Date);
                return _date;
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("private")]
        public bool isPrivate { get; set; }

        [JsonProperty("updated_at")]
        internal string _UpdateDate { get; set; }

        private DateTime _updatedate;
        public DateTime UpdateDate
        {
            get
            {
                if (_updatedate != DateTime.MinValue) return _updatedate;
                _updatedate = DateTime.Parse(_UpdateDate);
                return _updatedate;
            }
        }

        [JsonProperty("url_name")]
        public string UrlName { get; set; }
    }

    [JsonObject]
    public class Tags
    {
        [JsonProperty("name")]
        public string Name { get;private set; }

        public Tags(string name)
        {
            this.Name = name;
        }

        public static Tags[] BuildTags(params string[] tag)
        {
            var tags = tag.Select(value => new Tags(value)).ToArray();
            return tags;
        }
    }
}
