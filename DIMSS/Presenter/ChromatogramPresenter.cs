using DIMSS.Model;
using DIMSS.View;

namespace DIMSS.Presenter
{
    class ChromatogramPresenter
    {
        private readonly ChromatogramModel model = new ChromatogramModel();
        private readonly ChromatogramView view;

        public ChromatogramPresenter(ChromatogramView vw)
        {
            view = vw;
        }
    }
}
