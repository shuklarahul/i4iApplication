(function($) {
    Drupal.behaviors.v4 = {
        attach: function(context, settings) {
            $(".share").hideshare({
                media: $("#main .main-content .field-name-body img").attr("src")
            });
            $(".page-user-login, .page-user-password, .page-user-register").addClass("userForm");
            $(".page-user-login, .page-user-password, .page-user-register").find("nav, header, footer,.adsense, #sidebar, .feature").remove();

            $(".feature").show();
            $(".node-type-blog .cover").css("background", "url(" + $(".main-img-post img").attr("src") + ") center center");
            $(".node-type-blog .cover").css("background-size", "cover");
            $(".view .views-row").each(function(i) {
                if (i < 3) {
                    $(this).appendTo(".feature .wrap").addClass("adown");
                }

            })

            $(".page-user .tabs li a").each(function() {
                if ($(this).text() == "Log in") {
                    $(this).attr("href", $(this).attr("href") + "/login");
                }
            })

            $(".page-user-login .tabs li a.active").attr("href", $(".page-user-login .tabs li a.active").attr("href") + "/login");



            /* Tumblr */

            function tumblrload() {
                jjj = setTimeout(function() {
                    $(".tumblr_post").each(function() {
                        $(this).find("img").css("opacity", "0");
                        $(this).css("background", "url(" + $(this).find("img").attr("src") + ") center center");
                        $(this).css("background-size", "cover");
                    })
                    clearTimeout(jjj);
                }, 500)

            }

            /* Loading Animations */

            function animations() {
                xx = setTimeout(function() {
                    $(".adown").each(function(i) {
                        $(this).css({
                            y: "0"
                        }).transition({
                            opacity: 1,
                            y: "0px",
                            delay: 100 * i
                        }, 333, "ease", function() {

                        });
                        clearTimeout(xx);
                    })
                }, 150)

                xxx = setTimeout(function() {
                    $("nav.adown").removeAttr("style").css({
                        opacity: 1
                    });
                    clearTimeout(xxx);
                    loaded = true;
                }, 1000);
            }




            /* Drawer Animation */
            navStart = true;
            nav = false;
            $(".sidenav li").addClass("ani");
            $(".sidenav li li").removeClass("ani");

            function openDrawer() {
                $(".nav-menu").bind("click", function() {
                    if (!nav) {
                        $(this).addClass("close");
                        $("body").addClass("lockscroll");
                        $("footer, #main, .adsense, header, .feature, .cover, .nav-menu").css({
                            x: "0px"
                        }).transition({
                            x: "260px",
                            delay: 100
                        }, 600, "ease", function() {
                            //$("nav .region-header").appendTo(".sidenav");
                            //$(".sidena ul li li").removeAttr("style");
                            nav = true;
                            if (navStart) {
                                $(".sidenav li.ani").css({
                                    opacity: 0,
                                    y: "-10px"
                                }).each(function(i) {
                                    $(this).transition({
                                        opacity: 1,
                                        y: "0px",
                                        delay: 50 * (i)
                                    }, 100, "ease");
                                    navStart = false;
                                });
                            }
                        });
                        $(".sidenav").css({
                            x: "-250px"
                        }).transition({
                            x: "0"
                        }, 600, "ease", function() {

                        });
                    }


                    if (nav) {
                        $("body").removeClass("lockscroll");
                        $(this).removeClass("close");
                        $("footer, #main, .adsense, header, .feature, .cover, .nav-menu").transition({
                            x: "0px"
                        }, 600, "ease", function() {
                            //$(".sidenav .region-header").appendTo("nav");
                            //$(".sidenav li.ani").css({opacity:0, y:"-10px"});
                            nav = false;


                        });
                        $(".sidenav").transition({
                            x: "-250px",
                            delay: 100
                        }, 600, "ease");
                    }
                })

            }

            /* Basic Dropdowns */

            function dropdowns() {

                $("nav ul li.expanded, nav ul li.expanded ul").hover(
                    function() {
                        $("ul", this).stop(true, true).show();
                        $("ul", this).find("li").css({
                            opacity: 0
                        }).each(function(i) {
                            $(this).css({
                                y: "-10px",
                                rotateX: "90deg",
                                transformOrigin: 'center top'
                            }).transition({
                                perspective: '400px',
                                y: "0",
                                rotateX: "0deg",
                                opacity: 1,
                                delay: 50 * i
                            }, 200, "ease");
                        })

                    },
                    function() {
                        $("ul", this).stop(true, true).fadeOut(400);
                    }
                )
                $("nav ul li.expanded ul").hover(
                    function() {
                        $("ul", this).show();
                    },
                    function() {
                        $("ul", this).stop(true, true).fadeOut(400);
                    }
                )
                $("nav ul li").hover(function() {
                    //$("nav .block-search").hide();
                })




                $(".sidenav ul li.expanded").each(function() {
                    if ($(this).hasClass("leaf")) {}
                    $(this).find("a").first().removeAttr("href");
                });

                $(".sidenav ul li.expanded").click(function() {
                    if (!$(this).hasClass("selected")) {
                        $(".sidenav ul li.expanded").removeClass("selected");
                        $(".sidenav ul li.expanded ul").stop(true, true).hide();
                        $("ul", this).stop(true, true).show();
                        $("ul", this).find("li").css({
                            opacity: 0
                        }).each(function(i) {
                            $(this).css({
                                y: "-10px",
                                rotateX: "90deg",
                                transformOrigin: 'center top'
                            }).transition({
                                perspective: '400px',
                                y: "0",
                                rotateX: "0deg",
                                opacity: 1,
                                delay: 50 * i
                            }, 200, "ease");
                        })
                        $(this).addClass("selected");
                    }

                })

            }

            function searchMenu() {
                $("nav ul li.last.leaf").each(function() {
                    if ($(this).text() == "Search") {
                        $(this).find("a").removeAttr("href");
                    }
                })
                $("nav ul li.last.leaf").click(
                    function() {
                        if ($(this).text() == "Search") {
                            $("nav .block-search").css({
                                opacity: 0
                            }).show().transition({
                                opacity: 1
                            }, 300, "ease");
                            $("nav .block-search input.form-text").focus();
                        }
                    });
                $("header, .feature, .adsense, #main").click(
                    function() {
                        $("nav .block-search").hide();
                    })


                /* SEARCH CLICK 
                if (!navigator.userAgent.match(/(iPod|iPhone|iPad|Android)/)) {
                    $("nav ul li.last.leaf").each(function(){
                      if ($(this).text() == "Search") {
                        $(this).removeAttr("href");
                        alert()
                      }
                    })
                    $("nav ul li.last.leaf").click(
                        function() {
                            if ($(this).text() == "Search") {
                                $("nav .block-search").css({
                                    opacity: 0
                                }).show().transition({
                                    opacity: 1
                                }, 300, "ease");
                                $("nav .block-search input.form-text").focus();
                            }
                        });
                    $("nav input.form-text").hover(
                        function() {
                            $("nav .block-search").hide();
                        })
                }

                /* SEARCH HOVER 
                if (navigator.userAgent.match(/(iPod|iPhone|iPad|Android)/)) {
                    $("nav ul li.last.leaf").hover(
                        function() {
                            if ($(this).text() == "Search") {
                                $("nav .block-search").css({
                                    opacity: 0
                                }).show().transition({
                                    opacity: 1
                                }, 300, "ease");
                                $("nav .block-search input.form-text").focus();
                            }
                        },
                        function() {
                            //$("nav .block-search").hide();
                        }
                    )
                    $("nav input.form-text").hover(
                        function() {
                            $("nav .block-search").show();

                        },
                        function() {
                            $("nav .block-search").hide();
                        }
                    )
                }*/

            }







            window.onscroll = function() {
                if ($(window).width() > 767) {
                    if ($(window).scrollTop() >= 160) {
                        $("nav, body").addClass("top");
                    }
                    if ($(window).scrollTop() < 160) {
                        $("nav, body").removeClass("top");
                    }
                }
            }

            window.onresize = function() {
                if ($(window).width() > 768) {
                    $("footer, #main, .adsense, header, .feature, .nav-menu").transition({
                        x: "0px"
                    }, 250, "out");
                    $(".sidenav").transition({
                        x: "-310"
                    }, 250, "out");
                    $(".nav-menu").removeClass("close");
                    navStart = true;
                    nav = false;
                }
                if ($(window).width() < 768) {
                    $(".nav-menu").transition({
                        opacity: 1,
                        delay: 1000
                    }, 300, "ease");
                    responsiveBanner();
                }
            }
            if ($(window).width() < 768) {
                $(".nav-menu").transition({
                    opacity: 1,
                    delay: 1000
                }, 300, "ease");
                responsiveBanner()
            }

            function responsiveBanner() {
                ml = Math.round(768 * 0.43);
                sl = $(".adsense").width() - ml;
                $(".adframe").css("margin-left", (sl / 2) + "px");
            }



            animations();
            tumblrload();
            openDrawer();
            searchMenu();
            dropdowns();

        }
    };
})(jQuery);
