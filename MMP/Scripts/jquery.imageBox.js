(function($){
	$.fn.imageBox = function(options) {

		var shortcut= $.fn.imageBox;
		
		$.fn.imageBox.defaults = {
			closeMsg: "Cerrar",
			zoomIcon: "",
			zoomSmallIcon:"",
			loaderIcon: "",
			closeIcon: ""
		};
		var options = $.extend({}, $.fn.imageBox.defaults, options);
		
		if (options.zoomIcon=="")
		{
			$("script").each(function () { 
				if(this.src.toString().match(/jquery\.imageBox.*?js$/)) { options.zoomIcon = this.src.toString().replace(/jquery\.imageBox.*?js$/, "") + "imageBoxZoomIcon.png"; return false; }
			});
		}
		
		if (options.zoomSmallIcon=="")
		{
			$("script").each(function () { 
				if(this.src.toString().match(/jquery\.imageBox.*?js$/)) { options.zoomSmallIcon = this.src.toString().replace(/jquery\.imageBox.*?js$/, "") + "imageBoxZoomSmallIcon.png"; return false; }
			});
		}
		
		if (options.closeIcon=="")
		{
			$("script").each(function () { 
				if(this.src.toString().match(/jquery\.imageBox.*?js$/)) { options.closeIcon = this.src.toString().replace(/jquery\.imageBox.*?js$/, "") + "imageBoxCloseButton.png"; return false; }
			});
		}
		
		if (options.loaderIcon=="")
		{
			$("script").each(function () { 
				if(this.src.toString().match(/jquery\.imageBox.*?js$/)) { options.loaderIcon = this.src.toString().replace(/jquery\.imageBox.*?js$/, "") + "imageBoxLoaderIcon.gif"; return false; }
			});
		}
	
		return this.each(function() {
			if ($(this).attr("rel")!=undefined)
			{
				var imageName=$(this).attr("rel");
				var image=$(this);
				$(this).wrapAll('<div class="imageBoxDiv" style="width:'+$(this).width()+'px; height:'+$(this).height()+'px;"></div>');
				var divContainer=$(this).parents(".imageBoxDiv:eq(0)");
				divContainer.append('<div class="imageBoxZoomSmallIcon" style="position:relative; width:'+$(this).width()+'px; height:'+$(this).height()+'px; left:0px; top:-'+$(this).height()+'px; background-image: url('+options.zoomSmallIcon+'); background-repeat: no-repeat; background-position: right bottom; cursor:pointer;"> </div>');
				divContainer.append('<div class="imageBoxZoomIcon" style="position:relative; width:'+$(this).width()+'px; height:'+$(this).height()+'px; left:0px; top:-'+($(this).height()*2)+'px; background-image: url('+options.zoomIcon+'); background-repeat: no-repeat; background-position: center center; cursor:pointer; visibility:hidden;"> </div>');
				
				divContainer.bind("mouseenter",function () {
					var icon=$(this).find(".imageBoxZoomIcon");
					icon.css("opacity","0").css("visibility","visible");
					if($.browser.msie){
						icon.show().fadeTo(200, 0.6); // no llevamos el fade al 100% para que no salga el fondo negro (bug de IE)
					}
					else{
						icon.show().fadeTo(200, 1.0);
					}
				}).bind("mouseleave", function() {
					$(this).find(".imageBoxZoomIcon").fadeOut(100);
				}).bind("click",{imageName:imageName},zoomClick)
			}
		});
		
		/* ZOOMCLICK */
		function zoomClick(evt)
		{
			$("body").append('<div class="imageBoxBackground" style="position:absolute; width:'+$(window).width()+'px; height:'+$(window).height()+'px; z-index:99998; background-color:#FFF; left:0; top:0; display:none; background-image: url('+options.loaderIcon+'); background-repeat: no-repeat; background-position: center center; cursor:pointer;"></div>');
			$(window).bind("resize",resize);
			$(".imageBoxBackground").css('opacity','0').show().fadeTo(500,0.8, function() { // cuando termina el fadeTo carga la imagen				
				var image = new Image();
				$(image).load(function () {
					//$(this).css('display','none'); // since .hide() failed in safari
					if ($(this).attr('height')>$(window).height())
					{
						$(this).attr('width',$(this).attr('width')*($(window).height()-100)/$(this).attr('height'));
						$(this).attr('height',$(window).height()-100);
					}
					$("body").append('<div class="imageBoxImage" style="position:absolute; z-index:99999; background-color:#333; padding:10px; left:10px; display:none;"></div>');
					$("body").append('<div class="imageBoxCloseButton" style="position:absolute; z-index:100000; left:0; top:0; display:none;"><a style="cursor:pointer"><img src="'+options.closeIcon+'" width="20" height="20" alt="'+options.closeMsg+'"/></a></div>');
					$(".imageBoxImage").append($(this))
					.css("left",($(window).width()/2)-($(this).attr("width")/2))
					.css("top",($(window).height()/2)-($(this).attr("height")/2))
					.fadeIn("slow", function() {
						$(".imageBoxBackground").css("background-image","");
					});
					$(".imageBoxCloseButton").css("left",parseInt($(".imageBoxImage").css("left").replace("px",""))+$(".imageBoxImage").width()+10)
					.css("top",parseInt($(".imageBoxImage").css("top").replace("px",""))-10)
					.fadeIn("slow", function() {
						$(this).click(closeImageBox);
					});
				}).error(function() {
					alert('La imagen no existe');
					closeImageBox();
				}).attr('src', evt.data.imageName);
			}).click(closeImageBox);
		}
		
		/* CLOSEIMAGEBOX */
		function closeImageBox()
		{
			$(".imageBoxImage").fadeTo(500,0, function() {
				$(this).remove();
				$(".imageBoxBackground").fadeTo(500,0,function() {
					$(this).remove();
					$(window).unbind("resize",resize);
				});
			});
			$(".imageBoxCloseButton").fadeTo(250,0, function() {
				$(this).remove();
			});
		}
		
		/* RESIZE */
		function resize()
		{
			$(".imageBoxBackground").css("width",$(window).width()).css("height",$(window).height());
		}
		
		
	};
})(jQuery);