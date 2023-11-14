const list = $('.list');
const url = window.location.href;
$(document).ready(function () {
    $('.dinheiro').mask("#.##0,00", { reverse: true });
    $('.numero').mask('0#');
    list.each(function () {
        activelink($(this))
        var nome = $(this).text().trim()
        var ativo = url.indexOf(nome)
        if (ativo == -1) {
            $(this).removeClass('active');
        }
        else {
            $(this).addClass('active');
        }
})
  
});
var activelink = function (item) {
    item.on('click', function () {
        list.removeClass('active');
        item.addClass('active');
    })

};