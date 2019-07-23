///	<summary>
/// 中文转换
///	</summary>
var SwaggerTranslator = (function () {
    //定时执行检测是否转换成中文,最多执行500次  即500*50/1000=25s
    var getLangDesc = function (word) {
        return words[$.trim(word)] !== undefined ? words[$.trim(word)] : word;
    };
    var iexcute = 0,
    //中文语言包
    words = {
        "Warning: Deprecated": "警告：已过时",
        "Implementation Notes": "实现备注",
        "Response Class": "响应类",
        "Status": "状态",
        "Parameters": "参数",
        "Parameter": "参数",
        "Value": "值",
        "Description": "描述",
        "Parameter Type": "参数类型",
        "Data Type": "数据类型",
        "Response Messages": "响应消息",
        "HTTP Status Code": "HTTP状态码",
        "Reason": "原因",
        "Response Model": "响应模型",
        "Request URL": "请求URL",
        "Response Body": "响应体",
        "Response Code": "响应码",
        "Response Headers": "响应头",
        "Hide Response": "隐藏响应",
        "Headers": "头",
        "Try it out!": "试一下！",
        "Show/Hide": "显示/隐藏",
        "List Operations": "显示操作",
        "Expand Operations": "展开操作",
        "Raw": "原始",
        "can't parse JSON.  Raw result": "无法解析JSON. 原始结果",
        "Model Schema": "模型架构",
        "Model": "模型",
        "apply": "应用",
        "Username": "用户名",
        "Password": "密码",
        "Terms of service": "服务条款",
        "Created by": "创建者",
        "See more at": "查看更多：",
        "Contact the developer": "联系开发者",
        "api version": "api版本",
        "Response Content Type": "响应Content Type",
        "fetching resource": "正在获取资源",
        "fetching resource list": "正在获取资源列表",
        "Explore": "浏览",
        "Show Swagger Petstore Example Apis": "显示 Swagger Petstore 示例 Apis",
        "Can't read from server.  It may not have the appropriate access-control-origin settings.": "无法从服务器读取。可能没有正确设置access-control-origin。",
        "Please specify the protocol for": "请指定协议：",
        "Can't read swagger JSON from": "无法读取swagger JSON于",
        "Finished Loading Resource Information. Rendering Swagger UI": "已加载资源信息。正在渲染Swagger UI",
        "Unable to read api": "无法读取api",
        "from path": "从路径",
        "Click to set as parameter value": "点击设置参数",
        "server returned": "服务器返回"
    },

    //定时执行转换
     translator2Cn = function () {
         if ($("#resources_container .resource").length > 0) {
             tryTranslate();
         }

         if ($("#explore").text() == "Explore" && iexcute < 500) {
             iexcute++;
             setTimeout(translator2Cn, 50);
         }
     },

     //设置控制器注释
     setControllerSummary = function () {
         $.ajax({
             type: "get",
             async: true,
             url: $("#input_baseUrl").val(),
             dataType: "json",
             success: function (data) {
                 var summaryDict = data.ControllerDesc;
                 var id, controllerName, strSummary;
                 $("#resources_container .resource").each(function (i, item) {
                     id = $(item).attr("id");
                     if (id) {
                         controllerName = id.substring(9);
                         strSummary = summaryDict[controllerName];
                         if (strSummary) {
                             $(item).children(".heading").children(".options").prepend('<li class="controller-summary" title="' + strSummary + '">' + strSummary + '</li>');
                         }
                     }
                 });
             }
         });
     },

     //尝试将英文转换成中文
    tryTranslate = function () {
        $('[data-sw-translate]').each(function () {
            $(this).html(getLangDesc($(this).html()));
            $(this).val(getLangDesc($(this).val()));
            $(this).attr('title', getLangDesc($(this).attr('title')));
        });
    };

    return {
        Translator: function () {
            document.title = "诺优API描述文档";
            $('body').append('<style type="text/css">.controller-summary{color:#10a54a !importantword-break:keep-all;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;max-width:250px;text-align:right;cursor:default;}#header{padding: 40px 0 20px 0px !important;}.swagger-section #header #logo {font-size: 2em;font-weight: bold;text-decoration: none;padding: 50px 0 20px 250px;color: white;} .input{display:none !important;}</style>');
            var link = document.head.querySelector("link");
            link.href = "";
            $("#logo").html("诺优在线API文档").attr("href", "/swagger/ui/index").css({ "padding": "50px 0 20px 120px" });
            $(".info_title").html("API须知");

            var html = "";

            var localaddress = "http://" + location.host + "/" + "api/{Controller}/{Action}";
            html += "1.接口地址：" + localaddress + "<br/>";
            html += "2.请求参数说明:请求参数为加密JSON格式,<br/>";
            html += "3.输出参数说明:输出参数为加密JSON格式（Status>接口状态,Msg>接口状态信息，Data>返回值）<br/>";
            //html += "4.接口采用摘要验证，（所需账密请联系管理员)";

            $(".info_description.markdown").html(html);
            //设置控制器描述
            setControllerSummary();
            translator2Cn();
        }
    }
})();
//执行转换
SwaggerTranslator.Translator();







