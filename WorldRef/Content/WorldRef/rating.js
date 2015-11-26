/* jQuery Star Rating Plugin
 * 
 * @Author
 * Copyright Nov 02 2010, Irfan Durmus - http://irfandurmus.com/
 *
 * @Version
 * 0.3b
 *
 * @License
 * Dual licensed under the MIT or GPL Version 2 licenses.
 *
 * Visit the plugin page for more information.
 * http://irfandurmus.com/projects/jquery-star-rating-plugin/
 *
 */

;(function($){
    $.fn.rating = function(callback){       
        callback = callback || function (rate, e) {        
            SaveRating(rate);
        };

        // each for all item
        this.each(function(i, v){            
            $(v).data('rating', {callback:callback})
                .bind('init.rating', $.fn.rating.init)
                .bind('set.rating', $.fn.rating.set)
                .bind('hover.rating', $.fn.rating.hover)
                .trigger('init.rating');
        });
    };
    
    $.extend($.fn.rating, {
        init: function(e){
            var el = $(this),
                list = '',
                isChecked = null,
                childs = el.children(),
                i = 0,
                l = childs.length;
            
            for (; i < l; i++) {
                list = list + '<a class="star" title="' + $(childs[i]).val() + '" />';
                if ($(childs[i]).is(':checked')) {
                    isChecked = $(childs[i]).val();
                };
            };
            
            childs.hide();
            
            el
                .append('<div class="stars">' + list + '</div>')
                .trigger('set.rating', isChecked);
            
            $('a', el).bind('click', $.fn.rating.click);            
            el.trigger('hover.rating');
        },
        set: function(e, val) {
            var el = $(this),
                item = $('a', el),
                input = undefined;
            
            if (val) {
                item.removeClass('fullStar');
                
                input = item.filter(function(i){
                    if ($(this).attr('title') == val)
                        return $(this);
                    else
                        return false;
                });
                
                input
                    .addClass('fullStar')
                    .prevAll()
                    .addClass('fullStar');
            }
            
            return;
        },
        hover: function(e){
            var el = $(this),
                stars = $('a', el);
            
            stars.bind('mouseenter', function(e){
                // add tmp class when mouse enter
                $(this)
                    .addClass('tmp_fs')
                    .prevAll()
                    .addClass('tmp_fs');
                
                $(this).nextAll()
                    .addClass('tmp_es');
            });
            
            stars.bind('mouseleave', function(e){
                // remove all tmp class when mouse leave
                $(this)
                    .removeClass('tmp_fs')
                    .prevAll()
                    .removeClass('tmp_fs');
                
                $(this).nextAll()
                    .removeClass('tmp_es');
            });
        },
        click: function (e) {           
            e.preventDefault();
            var el = $(e.target),
                container = el.parent().parent(),
                inputs = container.children('input'),
                rate = el.attr('title'); 
            matchInput = inputs.filter(function (i) {              
                if ($(this).val() == rate)
                    return false;
                else
                    return false;
            });            
            matchInput
                .attr('checked', true)
				.siblings('input').attr('checked', false);
            
            container
                .trigger('set.rating', matchInput.val())
                .data('rating').callback(rate, e);
        }
    });
    
})(jQuery);




//function SaveRating(rate) {
//    $.fancybox.close();
//    var projectId = document.getElementById("projectId").value;
//    var ratiType=document.getElementById("ratingtype").value;
//        $.ajax({
//            type: "POST",
//            url: '/WorldRef/AddRating',
//            data: JSON.stringify({ "ProjectId": projectId, "Rating": rate, "RatingType": ratiType }),
//            contentType: 'application/json; charset=utf-8',
//            success: function (data) {
//                document.getElementById(projectId).innerHTML = "";
//                var result = "";
//                for (var j = 0; j <5 ; j++) {
//                    if(j<parseInt(data))
//                    {
//                        result += ' <a href="#inline1" class="fancybox">  <img src="/Content/WorldRef/FilledStar.png" alt="Star Rating" align="middle" id="@i" onclick="OpenPopUp(' + projectId + ');" /> </a>';
//                    }
//                    else
//                    {
//                        result += ' <a href="#inline1" class="fancybox">  <img src="/Content/WorldRef/EmptyStar.png" alt="Star Rating" align="middle" id="@i" onclick="OpenPopUp(' + projectId + ');"  /> </a>';
//                    }
//                }
//                for (var j = 0; j < 5 ; j++) {
//                    if (j < parseInt(data)) {
//                        result += ' <a href="#inline1" class="fancybox">  <img src="/Content/WorldRef/FilledStar.png" alt="Star Rating" align="middle" id="@i" onclick="OpenPopUpQuality(' + projectId + ');" /> </a>';
//                    }
//                    else {
//                        result += ' <a href="#inline1" class="fancybox">  <img src="/Content/WorldRef/EmptyStar.png" alt="Star Rating" align="middle" id="@i" onclick="OpenPopUpQuality(' + projectId + ');"  /> </a>';
//                    }
//                }
//                for (var j = 0; j < 5 ; j++) {
//                    if (j < parseInt(data)) {
//                        result += ' <a href="#inline1" class="fancybox">  <img src="/Content/WorldRef/FilledStar.png" alt="Star Rating" align="middle" id="@i" onclick="OpenPopUpUser(' + projectId + ');" /> </a>';
//                    }
//                    else {
//                        result += ' <a href="#inline1" class="fancybox">  <img src="/Content/WorldRef/EmptyStar.png" alt="Star Rating" align="middle" id="@i" onclick="OpenPopUpUser(' + projectId + ');"  /> </a>';
//                    }
//                }
//                document.getElementById(projectId).innerHTML = result;
//                window.location.href = '/WorldRef/ListExcelApprovedProjectAdmin';
//            }
//        });
//}