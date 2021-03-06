﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreQiita
{
    /// <summary>
    /// トークンクラス
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// 認証中のアクセストークン
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// ユーザーの操作
        /// </summary>
        public Users Users { get; } = new Users();

        /// <summary>
        /// 投稿記事の操作
        /// </summary>
        public Items Items { get; } = new Items();

        /// <summary>
        /// タグに関する操作
        /// </summary>
        public Tags Tags { get; } = new Tags();

        internal static HttpClient client = new HttpClient() {BaseAddress = new Uri(Url.BASE_URL) };

        /// <summary>
        /// アクセストークンで認証します
        /// </summary>
        /// <param name="token"></param>
        public Tokens(string token)
        {
            this.Token = token;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        /// <summary>
        /// 認証中のトークンを無効化します
        /// </summary>
        /// <returns>無効化の成否</returns>
        public bool TokenDelete()
        {
            return TokenDeleteAsync().Result;
        }

        /// <summary>
        /// 認証中のトークンを非同期で無効化します
        /// </summary>
        /// <returns>無効化の成否</returns>
        public async Task<bool> TokenDeleteAsync()
        {
            var message = await client.DeleteAsync($"api/v2/access_tokens/{Token}");
            return (int)message.StatusCode == 204;
        }
    }
}
