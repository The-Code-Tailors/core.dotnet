using System;
using System.Globalization;
using System.Net;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public class PercentagePropertyRenderer : PropertyRenderer
    {
        public PercentagePropertyRenderer(PropertyGridHelperProperty property) : base(property) { }

        public override object ParseValue(string s)
        {
            return double.Parse(s, CultureInfo.CurrentUICulture);
        }

        public override string RenderEditor(FormHelper formHelper)
        {
            return $@"
<div class=""form-group slider"">
	<label class=""control-label"" for=""{property.Identifier}"">{property.Label}</label>
	<a data-content=""{WebUtility.HtmlEncode(property.Description)}"" data-toggle=""popover"" data-trigger=""focus"" title=""{WebUtility.HtmlEncode(property.Label)}"" role=""button"" tabindex=""0""><span class=""glyphicon glyphicon-info-sign""></span></a>
	<input Id=""{property.Identifier}"" name=""{property.Identifier}"" type=""hidden"" value=""{Math.Round((double)property.Value, 2).ToString(CultureInfo.CurrentUICulture)}"" />
	<div id=""{property.Identifier}Slider""></div>
</div>
<script>
	$(function () {{
		$('#{property.Identifier}Slider').dxSlider({{
			min: 0,
			max: 100,
			value: {(int)Math.Round((double)property.Value * 100)},
			label: {{
				format: function (value) {{
					return value + '%';
				}},
				position: 'top',
				visible: true,
			}},
			tooltip: {{
				enabled: true,
				format: function (value) {{
					return value + '%';
				}},
				position: 'bottom',
				showMode: 'always',
			}},
			onValueChanged: function (data) {{
				$('#{property.Identifier}').val(Intl.NumberFormat('{CultureInfo.CurrentUICulture.Name}').format(data.value / 100));
			}},
		}});
	}});
</script>
";
        }

        public override string RenderViewer()
        {
            return ((double)property.Value).ToString("P0");
        }

    }
}

