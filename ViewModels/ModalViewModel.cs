using System.Collections.Generic;
using FluentBlog.Enum;
using FluentBlog.Models;

namespace FluentBlog.ViewModels
{
    public class ModalViewModel
    {
        public Meta SelectedMeta { get; set; }
        public Feed SelectedFeed { get; set; }
        public Friend SelectedFriend { get; set; }
        public ManageItemType ManageItemType { get; set; }
        public OperateType OperateType { get; set; }
    }
}