this["spa_templates"] = this["spa_templates"] || {};
this["spa_templates"]["templates"] = this["spa_templates"]["templates"] || {};
this["spa_templates"]["templates"]["catpic"] = this["spa_templates"]["templates"]["catpic"] || {};
this["spa_templates"]["templates"]["catpic"]["body"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<img width=\"250\" height=\"250\" alt=\"Vitra\" name=\"large\" src=\""
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"url") || (depth0 != null ? lookupProperty(depth0,"url") : depth0)) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"url","hash":{},"data":data,"loc":{"start":{"line":1,"column":60},"end":{"line":1,"column":67}}}) : helper)))
    + "\" style=\"display: block;\">";
},"useData":true});
Handlebars.registerPartial("fiche", Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div class=\""
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"kleur") || (depth0 != null ? lookupProperty(depth0,"kleur") : depth0)) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"kleur","hash":{},"data":data,"loc":{"start":{"line":1,"column":12},"end":{"line":1,"column":21}}}) : helper)))
    + "\">\r\n    \r\n </div>";
},"useData":true}));
this["spa_templates"]["templates"]["feedbackWidget"] = this["spa_templates"]["templates"]["feedbackWidget"] || {};
this["spa_templates"]["templates"]["feedbackWidget"]["body"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<section class=\"body\">\r\n    "
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"test") || (depth0 != null ? lookupProperty(depth0,"test") : depth0)) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"test","hash":{},"data":data,"loc":{"start":{"line":2,"column":4},"end":{"line":2,"column":12}}}) : helper)))
    + "\r\n </section>";
},"useData":true});
this["spa_templates"]["templates"]["speelbord"] = this["spa_templates"]["templates"]["speelbord"] || {};
this["spa_templates"]["templates"]["speelbord"]["body"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "        <tr>\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),depth0,{"name":"each","hash":{},"fn":container.program(2, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":5,"column":12},"end":{"line":9,"column":21}}})) != null ? stack1 : "")
    + "            \r\n        </tr>\r\n";
},"2":function(container,depth0,helpers,partials,data) {
    var stack1, helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "                <td class=\"cell\" id=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"row") || (depth0 != null ? lookupProperty(depth0,"row") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"row","hash":{},"data":data,"loc":{"start":{"line":6,"column":37},"end":{"line":6,"column":44}}}) : helper)))
    + alias4(((helper = (helper = lookupProperty(helpers,"column") || (depth0 != null ? lookupProperty(depth0,"column") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"column","hash":{},"data":data,"loc":{"start":{"line":6,"column":44},"end":{"line":6,"column":54}}}) : helper)))
    + "\">\r\n"
    + ((stack1 = container.invokePartial(lookupProperty(partials,"fiche"),depth0,{"name":"fiche","hash":{"kleur":(depth0 != null ? lookupProperty(depth0,"value") : depth0)},"data":data,"indent":"                        ","helpers":helpers,"partials":partials,"decorators":container.decorators})) != null ? stack1 : "")
    + "                </td>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<table>\r\n<tbody>\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"array") : depth0),{"name":"each","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":3,"column":4},"end":{"line":12,"column":13}}})) != null ? stack1 : "")
    + "</tbody>\r\n</table>\r\n\r\n";
},"usePartial":true,"useData":true});