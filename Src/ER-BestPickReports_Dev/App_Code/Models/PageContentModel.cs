namespace ER_BestPickReports_Dev.App_Code.Models
{
    public class PageContentModel
    {
        #region private

        private int _pageId;
        private string _pageName;
        private string _urlTitle;
        private string _pageContent;
        private string _metaTitle;
        private string _metaKeywords;
        private string _metaDesc;
        private bool _isMobileApp;
        private string _pageContentMobile;

        #endregion private

        #region public

        public int PageId
        {
            get { return _pageId; }
            private set { _pageId = value; }
        }
        public string PageName
        {
            get { return _pageName; }
            private set { _pageName = value; }
        }
        public string UrlTitle
        {
            get { return _urlTitle; }
            private set { _urlTitle = value; }
        }
        public string PageContent
        {
            get { return _pageContent; }
            private set { _pageContent = value; }
        }
        public string MetaTitle
        {
            get { return _metaTitle; }
            private set { _metaTitle = value; }
        }
        public string MetaKeywords
        {
            get { return _metaKeywords; }
            private set { _metaKeywords = value; }
        }
        public string MetaDesc
        {
            get { return _metaDesc; }
            private set { _metaDesc = value; }
        }
        public bool IsMobileApp
        {
            get { return _isMobileApp; }
            private set { _isMobileApp = value; }
        }
        public string PageContentMobile
        {
            get { return _pageContentMobile; }
            private set { _pageContentMobile = value; }
        }

        #endregion public

        public PageContentModel(){}

        public PageContentModel(int pageId, string pageName, string urlTitle
                                , string pageContent, string metaTitle, string metaKeywords
                                , string metaDesc, bool isMobile, string pageContentMobile)
        {
            _pageId = pageId;
            _pageName = pageName;
            _urlTitle = urlTitle;
            _pageContent = pageContent;
            _metaTitle = metaTitle;
            _metaKeywords = metaKeywords;
            _metaDesc = metaDesc;
            _isMobileApp = isMobile;
            _pageContentMobile = pageContentMobile;
        }
    }
}