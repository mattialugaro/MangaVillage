using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace MangaVillage.Views.Helpers
{
    public static class HtmlHelpers
    {
        // Helpers per la selezione multipla delle categorie e generi nella creazione e modifica di manga
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

        // Helpers per la visualizzazione degli avatar nella vista di modifica profilo 
        public static MvcHtmlString MostraImmagini(this HtmlHelper helper, List<string> nomiImmagini, string selectedImage = null)
        {
            var html = new StringBuilder();
            int i = 0;

            html.Append("<div class=\"row\">");
            selectedImage = selectedImage ?? nomiImmagini.FirstOrDefault();

            foreach (var nomeImmagine in nomiImmagini)
            {
                var pathImmagine = VirtualPathUtility.ToAbsolute("~/Content/Avatar/" + nomeImmagine);
                var isChecked = nomeImmagine == selectedImage;
                var radioButtonId = $"SelectedAvatar-{i++}";

                html.Append($@"
            <div class=""col-2"">
                <label for=""{radioButtonId}"">
                    <input type=""radio"" id=""{radioButtonId}"" name=""SelectedAvatar"" value={nomeImmagine} {(isChecked ? "checked" : "")} />
                    <img src=""{pathImmagine}"" alt=""{nomeImmagine}"" width=""100px"" height=""100px"" style=""object-fit:cover"" class=""rounded-circle"" />
                </label>
            </div>");
            }

            html.Append("</div>");

            return MvcHtmlString.Create(html.ToString());
        }





    }
}