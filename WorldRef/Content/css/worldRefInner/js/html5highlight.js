// JavaScript source code
(function ($) {

    $(document).ready(function () {
        $(".inlinecode").each(function () {

            $("pre", $(this)).each(function () {
                var $this = $(this);
                var content = $this.html().replace(/"([^"]*)"/g, '"<span style=\'color:#0000ff\'>$1</span>"');
                content = content.replace(/(&lt;\w+\s+)/g, '<span style=\'color:#ff0000\'>$1</span>');
                content = content.replace(/(&lt;\w+)(?=&gt;)/g, '<span style=\'color:#ff0000\'>$1</span>');
                content = content.replace(/(&lt;\/\w+&gt;)/g, '<span style=\'color:#ff0000\'>$1</span>');
                content = content.replace(/&gt;(?!<\/span>)/g, '<span style=\'color:#ff0000\'>&gt;</span>');
                $this.html(content);
            });
        });
    });
})(jQuery);
