(function($){
var $ = jQuery;
jQuery(document).ready(function ($) {
    var text=$('.menu-item').first().text(); //.toLowerCase() removed on request
    if(text == 'home'){
        $('.menu-item').first().find('a').html('<i class="icon-white icon-home"></i>')
    }
    
    $(".mega-drop").each(function () {
        var count=$(this).children().length;
        if(count == 3){$(this).addClass('threecolumns');}
        if(count == 2){$(this).addClass('twocolumns');}
        if(count == 1){$(this).addClass('onecolumns');}
    });

  $('.menu-item').has('.mega-drop').mouseenter(function(){
   var active=$(this);
    active.stop().find('.mega-drop').show('fast',function(){active.addClass('active');});
  });
  
  $('.menu-item').has('.mega-drop').mouseleave(function(){
  	var active=$(this);
    active.find('.mega-drop').stop().hide('fast',function(){active.removeClass('active');});
  });

$('.menu-item-depth-2').has('.mega-sub.menu-depth-3').first('a').addClass('levelmenu');

$('.nav-tabs a').click(function (e) {
  e.preventDefault();
  $(this).tab('show');
});


$('.nav-tabs').each(function(){
    $(this).find('a:first').tab('show');
    $(this).find('li:first').addClass('active');
});


jQuery(document).ready(function($){    
    
jQuery('#nav_container.fix_nav').each(function(){
var top = jQuery('#nav_container.fix_nav').offset().top;
	$(window).scroll(function(){
	  var scrolltop =$(window).scrollTop();
		if(  scrolltop > top){
			$('#nav_container.fix_nav').addClass('fix');
		}else{
			$('#nav_container.fix_nav').removeClass('fix');
		}
	});
   });     
});

    
if($('.fit_video').length)
$(".fit_video").fitVids();

$('.form-submit').addClass('offset2');

$('.fullwidthtop').each(function(){
    $element=$(this);
    if ($('.breadcrumbs').length > 0) {
         $('.breadcrumbs').after($element);
    }else{
        $('.main-section .row').before($element);
    }
});
});

jQuery(document).ready(function ($) {
 $('body').popover({
     selector: '[rel=popover]',
     placement: $(this).attr('data-placement')
 });


 $('body').tooltip({
     selector: '[rel=tooltip]',
     placement: $(this).attr('data-placement')
 });  
 
 $('.scrollspy').scrollspy();
 $('#navbar').scrollspy();  

 
 $('.animated_menu_default').iconmenu();
 $('.animated_menu_style1').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 400, easing : 'easeOutExpo', delay : 140, dir : 1},
						'sText' : {speed : 400, easing : 'easeOutExpo', delay : 0, dir : 1},
						'icon'  : {speed : 800, easing : 'easeOutBounce', delay : 280, dir : 1}
					},
					animMouseleave	: {
						'mText' : {speed : 400, easing : 'easeInExpo', delay : 140, dir : 1},
						'sText' : {speed : 400, easing : 'easeInExpo', delay : 280, dir : 1},
						'icon'  : {speed : 400, easing : 'easeInExpo', delay : 0, dir : 1}
					}
				});   

$('.animated_menu_style2').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 150, easing : 'jswing', delay : 0, dir : 1},
						'sText' : {speed : 150, easing : 'jswing', delay : 0, dir : 1},
						'icon'  : {speed : 150, easing : 'jswing', delay : 0, dir : 1}
					},
					animMouseleave	: {
						'mText' : {speed : 100, easing : 'jswing', delay : 0, dir : 1},
						'sText' : {speed : 100, easing : 'jswing', delay : 0, dir : 1},
						'icon'  : {speed : 100, easing : 'jswing', delay : 0, dir : 1}
					},
					boxAnimSpeed: 0
				});
$('.animated_menu_style3').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 200, easing : 'easeOutExpo', delay : 250, dir : 1},
						'sText' : {speed : 200, easing : 'easeOutExpo', delay : 0, dir : 1},
						'icon'  : {speed : 200, easing : 'easeOutExpo', delay : 500, dir : 1}
					},
					animMouseleave	: {
						'mText' : {speed : 200, easing : 'easeInExpo', delay : 250, dir : 1},
						'sText' : {speed : 200, easing : 'easeInExpo', delay : 500, dir : 1},
						'icon'  : {speed : 200, easing : 'easeInExpo', delay : 0, dir : 1}
					}
				});
$('.animated_menu_style4').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 200, easing : 'easeOutExpo', delay : 0, dir : 1},
						'sText' : {speed : 600, easing : 'easeOutExpo', delay : 400, dir : 1},
						'icon'  : {speed : 200, easing : 'easeOutExpo', delay : 0, dir : 1}
					},
					animMouseleave	: {
						'mText' : {speed : 200, easing : 'easeInExpo', delay : 150, dir : 1},
						'sText' : {speed : 200, easing : 'easeInExpo', delay : 0, dir : 1},
						'icon'  : {speed : 200, easing : 'easeInExpo', delay : 300, dir : 1}
					}
				});   
$('.animated_menu_style5').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 400, easing : 'easeOutExpo', delay : 140, dir : -1},
						'sText' : {speed : 400, easing : 'easeOutExpo', delay : 280, dir : -1},
						'icon'  : {speed : 400, easing : 'easeOutExpo', delay : 0, dir : -1}
					},
					animMouseleave	: {
						'mText' : {speed : 400, easing : 'easeInExpo', delay : 140, dir : -1},
						'sText' : {speed : 400, easing : 'easeInExpo', delay : 0, dir : -1},
						'icon'  : {speed : 400, easing : 'easeInExpo', delay : 280, dir : -1}
					}
				});
$('.animated_menu_style6').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 400, easing : 'easeOutBack', delay : 200, dir : -1},
						'sText' : {speed : 400, easing : 'easeOutBack', delay : 400, dir : -1},
						'icon'  : {speed : 400, easing : 'easeOutBack', delay : 0, dir : -1}
					},
					animMouseleave	: {
						'mText' : {speed : 200, easing : 'easeInExpo', delay : 150, dir : 1},
						'sText' : {speed : 200, easing : 'easeInExpo', delay : 300, dir : 1},
						'icon'  : {speed : 200, easing : 'easeInExpo', delay : 0, dir : 1}
					}
				});
$('.animated_menu_style7').iconmenu({
					animMouseenter	: {
						'mText' : {speed : 500, easing : 'easeOutExpo', delay : 200, dir : -1},
						'sText' : {speed : 500, easing : 'easeOutExpo', delay : 200, dir : -1},
						'icon'  : {speed : 700, easing : 'easeOutBounce', delay : 0, dir : 1}
					},
					animMouseleave	: {
						'mText' : {speed : 400, easing : 'easeInExpo', delay : 0, dir : -1},
						'sText' : {speed : 400, easing : 'easeInExpo', delay : 0, dir : 1},
						'icon'  : {speed : 400, easing : 'easeInExpo', delay : 0, dir : -1}
					}
				});                                

 });
 
jQuery(window).load(function ($) {
    var $ = jQuery;
   
    var vcoptions;
    var ssoptions;
    var vctoptions;
    var sstoptions;
    var svsoptions;
    var sssoptions;

 if(options != null){
        vcoptions=options;
        ssoptions=options;
        vctoptions=options;
        sstoptions=options;
        svsoptions=options;
        sssoptions=options;
    }
   
   
  var vcset={
    animation: "fade",
    start: function(){vibecom_init();},
    before: function(){vibecom_begin();},
    after: function(){vibecom_slide();}
  };
$.extend(true, vcoptions, vcset); 

if ($('.vibecom-slider').attr('data-id') !== undefined){
var sliderid=eval('slideroptions'+$('.vibecom-slider').attr('data-id'));
if(typeof sliderid !== "undefined"){
$.extend(true, vcoptions,sliderid ); 
}
}

$('.vibecom-slider').flexslider(vcoptions);

// Simple Slider Settings

  var ss={
    animation: "fade",
    controlNav: true
  };
 $.extend(true, ssoptions, ss);   
 
 if ($('.simple-slider').attr('data-id') !== undefined){
 var sliderid=eval('slideroptions'+$('.simple-slider').attr('data-id'));
if(typeof sliderid !== "undefined"){
$.extend(true, ssoptions,sliderid );
    }
}

 $('.simple-slider').flexslider(ssoptions);
  
  
// Simple Thumbnail Slider  
  $.extend(true, sstoptions, tss);  
  if ($('.thumbnail-simple-slider').attr('data-id') !== undefined){
  var sliderid=eval('slideroptions'+$('.thumbnail-simple-slider').attr('data-id'));
if(typeof sliderid !== "undefined"){
$.extend(true, sstoptions,sliderid ); 
}}
  var tss={
    animation: "fade",
    controlNav: "thumbnails"
  };
  $.extend(sstoptions,tss ); 
  $('.thumbnail-simple-slider').flexslider(sstoptions);
 
 /*=== VibeCom thumbnails slider ====*/
 
  if ($('.thumbnail-vibecom-slider').attr('data-id') !== undefined){
  var sliderid=eval('slideroptions'+$('.thumbnail-vibecom-slider').attr('data-id'));
 if(typeof sliderid !== "undefined"){
 $.extend(true, vctoptions,sliderid ); 
 }}

var tvs={
    animation: "fade",
    controlNav: "thumbnails",
    start: function(){vibecom_init();},
    before: function(){vibecom_begin();},
    after: function(){vibecom_slide();}
  }; 
  $.extend(vctoptions, tvs); 
  
$('.thumbnail-vibecom-slider').flexslider(vctoptions);  
    
    // The slider being synced must be initialized first
    
    /*===Simple Step Slider===*/


    
    $('.stepcarousel').flexslider({
      animation: "slide",
      directionNav:false,
      controlNav:false,
      animationLoop: false,
      slideshow: false,
      itemWidth: stepwidth,
      itemMargin: 0,
      asNavFor: '.step-simple-slider'
    });
     
     
     var sss={
        animation: "fade",
      directionNav:true,
      controlNav:false,
      sync: ".stepcarousel"
  }; 
   $.extend(true, sssoptions,sss);  
   if ($('.step-simple-slider').attr('data-id') !== undefined){
  var sliderid=eval('slideroptions'+$('.step-simple-slider').attr('data-id'));
 if(typeof sliderid !== "undefined"){
 $.extend(true, sssoptions,sliderid ); 
 }}
  
    $('.step-simple-slider').flexslider(sssoptions);
    
    
             
$('.vstepcarousel').flexslider({
      animation: "slide",
      directionNav:false,
      controlNav:false,
      animationLoop: false,
      slideshow: false,
      itemWidth: stepwidth,
      itemMargin: 0,
      asNavFor: '.step-vibecom-slider'
    });

var svs={animation: "fade",
  controlNav:false,
  sync: ".vstepcarousel",
  start: function(){vibecom_init();},
before: function(){vibecom_begin();},
after: function(){vibecom_slide();}
}
$.extend(true, svsoptions,svs); 
if ($('.step-vibecom-slider').attr('data-id') !== undefined){
  var sliderid=eval('slideroptions'+$('.step-vibecom-slider').attr('data-id'));
 if(typeof sliderid !== "undefined"){
 $.extend(true, svsoptions,sliderid ); 
 }}
$('.step-vibecom-slider').flexslider(svsoptions);


/*=== CAROUSELs ===*/

// Thumbnail Carousels

var fwcc2={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 460,
    minItems: 2
  };
if($('.fullwidth .carousel_columns2').length > 0){
    var custom=eval('op'+$('.fullwidth .carousel_columns2').attr('id')); 
    $.extend(fwcc2,custom); 
    $('.fullwidth .carousel_columns2').flexslider(fwcc2);
}  




var sp3cc2={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 110,
    minItems: 1
  };
if($('.span3 .carousel_columns2').length > 0){
    var custom=eval('op'+$('.span3 .carousel_columns2').attr('id')); 
$.extend(sp3cc2,custom); 

$('.span3 .carousel_columns2').flexslider(sp3cc2); 
}  
 


var sp4cc2={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 150,
    minItems: 1
  };
if($('.span4 .carousel_columns2').length > 0){  
var custom=eval('op'+$('.span4 .carousel_columns2').attr('id')); 
$.extend(sp4cc2,custom); 
$('.span4 .carousel_columns2').flexslider(sp4cc2);
}

var sp6cc2={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 225,
    minItems: 2
  };
if($('.span6 .carousel_columns2').length > 0){  
var custom=eval('op'+$('.span6 .carousel_columns2').attr('id')); 
$.extend(sp6cc2,custom); 
$('.span6 .carousel_columns2').flexslider(sp6cc2);
}


var sp8cc2={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 305,
    minItems: 2
  };
 if($('.span8 .carousel_columns2').length > 0){ 
  var custom=eval('op'+$('.span8 .carousel_columns2').attr('id')); 
$.extend(sp8cc2,custom); 
$('.span8 .carousel_columns2').flexslider(sp8cc2);
 }
   
  
var sp9cc2={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 345,
    minItems: 2
  };
if($('.span9 .carousel_columns2').length > 0){  
var custom=eval('op'+$('.span9 .carousel_columns2').attr('id')); 
$.extend(sp9cc2,custom);   

$('.span9 .carousel_columns2').flexslider(sp9cc2);  
}

var fwcc3={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 307,
    minItems: 3
  };
if($('.fullwidth .carousel_columns3').length > 0){  
var custom=eval('op'+$('.fullwidth .carousel_columns3').attr('id')); 
$.extend(fwcc3,custom); 
$('.fullwidth .carousel_columns3').flexslider(fwcc3);
}

var sp4cc3={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 100,
    minItems: 2
  };
if($('.span4 .carousel_columns3').length > 0){  
var custom=eval('op'+$('.span4 .carousel_columns3').attr('id')); 
$.extend(sp4cc3,custom); 
$('.span4 .carousel_columns3').flexslider(sp4cc3);
}

var sp6cc3={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 150,
    minItems: 2
  };
  if($('.span6 .carousel_columns3').length > 0){  
var custom=eval('op'+$('.span6 .carousel_columns3').attr('id')); 
$.extend(sp6cc3,custom); 
$('.span6 .carousel_columns3').flexslider(sp6cc3);
  }

var sp8cc3={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 205,
    minItems: 2
  };
if($('.span8 .carousel_columns3').length > 0){    
var custom=eval('op'+$('.span8 .carousel_columns3').attr('id')); 
$.extend(sp8cc3,custom); 

  $('.span8 .carousel_columns3').flexslider(sp8cc3);
}

  var sp9cc3={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 230,
    minItems: 1
  };
if($('.span9 .carousel_columns3').length > 0){    
var custom=eval('op'+$('.span9 .carousel_columns3').attr('id')); 
$.extend(sp9cc3,custom); 

  $('.span9 .carousel_columns3').flexslider(sp9cc3);
}

var fwcc4={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 230,
    minItems: 2
  };
if($('.fullwidth .carousel_columns4').length > 0){    
var custom=eval('op'+$('.fullwidth .carousel_columns4').attr('id')); 
$.extend(fwcc4,custom); 
$('.fullwidth .carousel_columns4').flexslider(fwcc4); 
}

var sp4cc4={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 75,
    minItems: 1
  };
if($('.span4 .carousel_columns4').length > 0){    
var custom=eval('op'+$('.span4 .carousel_columns4').attr('id')); 
$.extend(sp4cc4,custom);   
$('.span4 .carousel_columns4').flexslider(sp4cc4);
}

var sp6cc4={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 113,
    minItems: 2
  };
if($('.span6 .carousel_columns4').length > 0){      
var custom=eval('op'+$('.span6 .carousel_columns4').attr('id')); 
$.extend(sp6cc4,custom); 
$('.span6 .carousel_columns4').flexslider(sp6cc4);  
}

 var sp8cc4={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 155,
    minItems: 2
  };
if($('.span8 .carousel_columns4').length > 0){      
var custom=eval('op'+$('.span8 .carousel_columns4').attr('id')); 
$.extend(sp8cc4,custom); 
 $('.span8 .carousel_columns4').flexslider(sp8cc4); 
}

var sp9cc4={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 175,
    minItems: 2
  };
if($('.span9 .carousel_columns4').length > 0){      
var custom=eval('op'+$('.span9 .carousel_columns4').attr('id')); 
$.extend(sp9cc4,custom); 
  $('.span9 .carousel_columns4').flexslider(sp9cc4); 
}

  var fwcc1={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 940,
    minItems: 1
  };
if($('.fullwidth .carousel_columns1').length > 0){      
var custom=eval('op'+$('.fullwidth .carousel_columns1').attr('id')); 
$.extend(fwcc1,custom); 
  $('.fullwidth .carousel_columns1').flexslider(fwcc1);
}

  var sp3cc1={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 240,
    minItems: 1
  };
if($('.span3 .carousel_columns1').length > 0){      
var custom=eval('op'+$('.span3 .carousel_columns1').attr('id')); 
$.extend(sp3cc1,custom); 
  $('.span3 .carousel_columns1').flexslider(sp3cc1);
}
  
  
  var sp4cc1={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 360,
    minItems: 1
  };
if($('.span4 .carousel_columns1').length > 0){        
var custom=eval('op'+$('.span4 .carousel_columns1').attr('id')); 
$.extend(sp4cc1,custom); 
}

  $('.span4 .carousel_columns1').flexslider(sp4cc1);
  
  
    var sp6cc1={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 460,
    minItems: 1
  };
if($('.span6 .carousel_columns1').length > 0){        
var custom=eval('op'+$('.span6 .carousel_columns1').attr('id')); 
$.extend(sp6cc1,custom); 
  $('.span6 .carousel_columns1').flexslider(sp6cc1);
}

  var sp8cc1={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 640,
    minItems: 1
  };
if($('.span8 .carousel_columns1').length > 0){        
var custom=eval('op'+$('.span8 .carousel_columns1').attr('id')); 
$.extend(sp8cc1,custom);  
  $('.span8 .carousel_columns1').flexslider(sp8cc1);
}
  
    var sp9cc1={
    animation: "slide",
    controlNav: true,
    directionNav: false,
    animationLoop: false,
    slideshow: false,
    itemWidth: 800,
    minItems: 1
  };
if($('.span9 .carousel_columns1').length > 0){        
var custom=eval('op'+$('.span9 .carousel_columns1').attr('id')); 
$.extend(sp9cc1,custom); 

  $('.span9 .carousel_columns1').flexslider(sp9cc1);
}
  
// The slider being synced must be initialized first

  
$('.thumb_slider').carousel({
interval: false
});  

  
  $('.portfolio_carousel').flexslider({
      animation: "slide",
      controlNav: true,
      directionNav: false,
      animationLoop: false,
      slideshow: false,
      itemWidth: 230
    });  
 $('.archive_carousel').flexslider({
      animation: "slide",
      controlNav: true,
      directionNav: false,
      animationLoop: false,
      slideshow: false,
      itemWidth: 220
    });
    
 $('.widget_carousel').each(function(){
   var auto=false;
   var loop=false;
   
   if($(this).hasClass('auto'))
      auto=true; 
    
    if($(this).hasClass('loop'))
      loop=true;
  
 $('.widget_carousel').flexslider({
     animation: "slide",
     slideshow: auto,                //Boolean: Animate slider automatically
     slideshowSpeed: 3000, 
     controlNav: true,
     directionNav: false,
     animationLoop: loop,
     itemWidth: 280
   });
  });
  
  $('.twitter_carousel').flexslider({
      animation: "slide",
      controlNav: false,
      directionNav: false,
      animationLoop: true,
      slideshow: true,
      itemWidth: 280
    });    

    
    $('.post_thumbslider').flexslider({
      animation: "fade",
      controlNav: false,
      animationLoop: false,
      slideshow: true
    });
    
    $('.projectitem').mouseenter(function(){
     this.append('<div class="overlay"><span>view details</span></div>');
    }); 
    $('.projectitem').mouseleave(function(){
     this.find('.overlay').remove();
    });
});

jQuery(document).ready(function ($) {
$('.hover_top_in').mouseenter(function(){
 		$(this).append('<span style="height:0"><i class="magnify"></i></span>');
 		$(this).find('span').animate(	{height: '100%', opacity: 0.8},400);
});

$('.hover_top_in').mouseleave(function(){
   $(this).find('span').animate({height: 0, opacity: 0},400);
	$(this).find('span').remove();
  });
  
  $('.hover_left_in').mouseenter(function(){
   		$(this).append('<span style="width:0"><i class="magnify"></i></span>');
   		$(this).find('span').animate(	{width: '100%', opacity: 0.8},400);
  });
  
  $('.hover_left_in').mouseleave(function(){
     $(this).find('span').animate({width: 0, opacity: 0},400);
  	$(this).find('span').remove();
    });
  
  $('.hover_fade_in').mouseenter(function(){
   		$(this).append('<span style="opacity:0"><i class="magnify"></i></span>');
   		$(this).find('span').animate({opacity: 0.8},400);
  });
  
  $('.hover_fade_in').mouseleave(function(){
     $(this).find('span').animate({opacity: 0},400);
  	$(this).find('span').remove();
    });
  
  $('.hover_video_top_in').mouseenter(function(){
   		$(this).append('<span style="height:0"><i class="magnify"></i></span>');
   		$(this).find('span').animate(	{height: '100%', opacity: 0.8},400);
  });
  
  $('.hover_video_top_in').mouseleave(function(){
     $(this).find('span').animate({height: 0, opacity: 0},400);
  	$(this).find('span').remove();
    });
    
    $('.hover_video_left_in').mouseenter(function(){
     		$(this).append('<span style="width:0"><i class="magnify"></i></span>');
     		$(this).find('span').animate(	{width: '100%', opacity: 0.8},400);
    });
    
    $('.hover_video_left_in').mouseleave(function(){
       $(this).find('span').animate({width: 0, opacity: 0},400);
    	$(this).find('span').remove();
      });
    
});

jQuery(window).load(function ($) {
 var $ = jQuery;
    var $container = $('.filterableitems_container'),
    	$filtersdiv = $('.vibe_filterable'),
        $checkboxes = $('.vibe_filterable a');
    

    
    $container.isotope({
      itemSelector: '.filteritem'
    }); 
    
    $checkboxes.click(function(event){
      event.preventDefault();
      var me = $(this);
      $filtersdiv.find('.active').removeClass();
      var filters = me.attr('data-filter');
      me.parent().addClass('active');
      $container.isotope({filter: filters});
    });
   
   $('.vibe_filterable .all').trigger('click');
   
});
jQuery(window).load(function ($) {
    jQuery('.filteritem .categories a').click(function(){
       var filterdata=jQuery(this).attr('data-filter') ;
       jQuery('.vibe_filterable a[data-filter*="'+filterdata+'"]').trigger('click');
   });
});
  
jQuery(document).ready(function ($) {
  	  $('.show_nav').click(function(){
  	  	$(this).next('ul').slideToggle('slow');
  	  	$(this).find('i').toggleClass('icon-minus');
  	  }); 	
  	  
      $('.topnav li > ul').hide();    //hide all nested ul's
      $('.topnav li > ul li a[class=current]').parents('ul').show().prev('a').addClass('vaccordionExpanded');  //show the ul if it has a current link in it (current page/section should be shown expanded)
      $('.topnav li:has(ul)').addClass('vaccordion');
      $('.topnav li:has(ul) > a').append('<i class="icon-plus icon-white"></i>');  

      $('.topnav li:has(ul) > a').click(function() {
          $(this).toggleClass('vaccordionExpanded'); //for CSS bgimage, but only on first a (sub li>a's don't need the class)
          $(this).find('i').toggleClass('icon-minus');
          $(this).next('ul').slideToggle('fast');
          $(this).parent().siblings('li').children('ul:visible').slideUp('fast')
              .parent('li').find('a').removeClass('vaccordionExpanded')
              .parent('li').find('i').removeClass('icon-minus');
          return false;
      });
      
      $('.toparrow a').click(function(event){ 		   
      event.preventDefault();
      $('body,html').animate({
      				scrollTop: 0
      			}, 800);
      			return false;
      });
  });
  
  var fixed = false;
  

  
  
  //SUBSCRIPTION FORM
  
jQuery(document).ready(function ($) {
  	// TIMER SECTION
  	var newYear = new Date(); 
  	var date = new Date('2013, 12, 1');//Change countdown date here
  	$("#timer").countdown({until: date, format: 'dHMS'}); 
  });


//AJAX CONTACT FORM
jQuery(document).ready(function ($) {
	
	// SUBSCRIPTION FORM AJAX HANDLE
	 
	          $(".send_message").click(function (event) {
                      event.preventDefault();
	              var valid = "";
	              $id=$(this).attr('data-id');
	              var $response= $(".response"+$id);
	              
	              var mail = $("#contact_email"+$id).val();
	              var $response= $(".response"+$id);
	              if (!mail.match(/^([a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,4}$)/i)) {
	                  valid += ' Invalid email';
	              }
	              if (valid !== "") {
	                  $response.fadeIn("slow");
	                  $response.html("<span style='color:#D03922;'>Error: " + valid + "</span>");
	              } else {
	              var name,field_label,field,message;
	              
	                  $response.css("display", "block");
	                  $response.html("<span style='color:#0E7A00;'>Sending message... </span>");
	                  $response.fadeIn("slow");
	                  name = $("#contact_name"+$id).val();
                          field_label=$("#contact_field"+$id).attr('placeholder');
	                  field = $("#contact_field"+$id).val();
	                  message = $("#contact_message"+$id).val();
	                  setTimeout(function(){sendmail($id,name,mail,field_label,field,message);}, 2000);
	              }
	              return false;
	          });
	          
	      
	      
	      function sendmail(id,name,mail,field_label,field,message) { 
	      	var $response= $(".response"+id);
	      	$.ajax({
	              type: "POST",
	              url: ajaxurl,
                      data: {action: 'contact_submission', 
                                email:mail,
                                name:name,
                                field_label:field_label,
                                field:field,
                                message:message},
	              cache: false,
	              success: function (html) {
	                  $response.fadeIn("slow");
	                  $response.html(html);
	                  var responsestring='$(".response'+id+'").fadeOut("slow")';
	                  setTimeout(responsestring, 10000);
	              }
	          });
	      }
	     
});

jQuery(document).ready(function ($) {
	   $( 'audio' ).audioPlayer(); 
       $('.gallery a').touchTouch();
});

jQuery(document).ready(function ($) {
	  $('.floatleft').each(function(){
	    var $this = $(this).parent();
	    if($this.hasClass('v_column')){
	      $this.addClass('left');
	    }
	  });
	  $('.floatright').each(function(){
	    var $this = $(this).parent();
	    if($this.hasClass('v_column')){
	      $this.addClass('right');
	    }
	  });
});
jQuery(document).ready(function ($) {
$("a[rel^='prettyPhoto']").prettyPhoto();
});

jQuery(document).ready(function ($) {
$('.button[href*="#modal"]').each(function() {
   $(this).attr('data-toggle','modal');
}); 
});

jQuery(document).scroll(function ($) {
      if( jQuery(this).scrollTop() > 150 ) {
          if( !fixed ) {
              fixed = true;
              jQuery('.toparrow').show('fast');
          }
      } else {
          if( fixed ) {
              fixed = false;
              jQuery('.toparrow').hide('fast')
          }
      }
       
  });

jQuery(document).ready(function ($) {
    $('#show_register').click(function(e){
       //e.preventDefault();
       $('.logintab').fadeOut();
        $('.registertab').fadeIn();
    });
    $('#show_login').click(function(e){
       //e.preventDefault();
       $('.registertab').fadeOut();
        $('.logintab').fadeIn();
    });
    
    $('.checkout_steps li').click(function(){
       var index= $(this).index();
       if(index < ($('.checkout_steps li').length-1) && index > 0){
            $('.step').fadeOut(200);
            $('.step.step'+index).fadeIn(200);
            $('.checkout_steps').find('.active').removeClass('active');
            $('.checkout_steps li:lt('+index+')').addClass('active');
            $(this).addClass('active');
       }
    });
    
    $('.proceed .btn').live("click", function () {
        var index=$(this).attr('rel');
            $('.step').fadeOut(200);
            $('.step.step'+index).fadeIn(200);
            $('.checkout_steps').find('.active').removeClass('active');
            $('.checkout_steps li:lt('+index+')').addClass('active');
            $('.checkout_steps li:eq('+index+')').addClass('active');
    });
    
    $('.removesearch').click(function(){
       $('#search').val(''); 
    });
  });
  
  
 //AJAX CONTACT FORM
jQuery(document).ready(function ($) {
	
	// SUBSCRIPTION FORM AJAX HANDLE
	         $( 'body' ).delegate( '.form .form_submit', 'click', function(event){
                      event.preventDefault();
                      var parent = $(this).parent();
	              var $response= parent.find(".response");
                      var error= '';
                      var to = []
                      var data = [];
                      var label = [];
	              var regex = [];
                      var to = parent.attr('data-to');
                      var subject = parent.attr('data-subject');
                      regex['email'] = /^([a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,4}$)/i;
                      regex['phone'] = /[A-Z0-9]{7}|[A-Z0-9][A-Z0-9-]{7}/i;
                      regex['numeric'] = /^[0-9]+$/i;
                      var i=0;
                      parent.find('.form_field').each(function(){
                          i++;
                          var validate=$(this).attr('data-validate');
                          var value = $(this).val();
                          if(!value.match(regex[validate])){
                              error += 'Invalid '+validate;
                              $(this).css('border-color','#e16038');
                          }else{
                              data[i]=value;
                              label[i]=$(this).attr('placeholder');
                          }
                      });
                          if (error !== "") {
	                  $response.fadeIn("slow");
	                  $response.html("<span style='color:#D03922;'>Error: " + error + "</span>");
                            } else {
                        $response.css("display", "block");
	                  $response.html("<span style='color:#0E7A00;'>Sending message... </span>");
	                  $response.fadeIn("slow");
                          setTimeout(function(){sendmail(to,subject,data,label,parent);}, 2000);
	              }
                      
	              return false;
	          });
	          
	      
	      
	      function sendmail(to,subject,formdata,labels,parent) { 
	      	var $response= parent.find(".response");
	      	$.ajax({
	              type: "POST",
	              url: ajaxurl,
                      data: {   action: 'vibe_form_submission', 
                                to: to,
                                subject : subject,
                                data:JSON.stringify(formdata),
                                label:JSON.stringify(labels)
                            },
	              cache: false,
	              success: function (html) {
	                  $response.fadeIn("slow");
	                  $response.html(html);
	                  setTimeout(function(){$response.fadeOut("slow");}, 10000);
	              }
	          });
	      }
	     
});
})(jQuery);