using API.Entities;
using API.Entities.ViewModels;
using API.Infra;

namespace API.Services
{
    public class NewsService
    {
        private readonly IRepository<News> _news;

        public NewsService(IRepository<News> news)
        {
            _news = news;
        }

        public Result<News> Get(int page, int qtd)
        {
            return _news.Get(page, qtd);
        }

        public News Get(string id)
        {
            return _news.Get(id);
        }



        public News Create(NewsViewModel news)
        {

            var entity = new News(news.Title, news.Status);

            _news.Create(entity);

            return Get(entity.Id);
        }

        public void Update(string id, NewsViewModel newsVM)
        {
            var news = Get(id);
            news.Title = newsVM.Title;
            news.Status = newsVM.Status;
            
            _news.Update(id, news);

        }

        public void Remove(string id)
        {

            var gallery = Get(id);
            _news.Remove(id);
        }

    }
}
