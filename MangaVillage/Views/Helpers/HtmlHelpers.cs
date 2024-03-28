using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangaVillage.Views.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MultiSelectList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object selectedValues = null, string multipleText = "Seleziona...")
        {
            var listBox = new TagBuilder("select");
            listBox.Attributes.Add("name", name);
            listBox.Attributes.Add("multiple", "multiple");

            if (selectedValues != null)
            {
                var selectedValuesList = new List<string>();
                if (selectedValues is IEnumerable<string>)
                {
                    selectedValuesList = (List<string>)selectedValues;
                }
                else if (selectedValues is string)
                {
                    selectedValuesList.Add((string)selectedValues);
                }

                foreach (var item in selectList)
                {
                    if (selectedValuesList.Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                }
            }

            // Opzione "Seleziona..."
            var defaultItem = new SelectListItem
            {
                Text = multipleText,
                Value = "",
                Selected = selectedValues == null
            };
            selectList = selectList.Prepend(defaultItem);

            listBox.InnerHtml = string.Join("", selectList.Select(item =>
                   $"<option value=\"{item.Value}\"{(item.Selected ? " selected" : "")}>{item.Text}</option>"));


            return MvcHtmlString.Create(listBox.ToString());
        }
    }
}