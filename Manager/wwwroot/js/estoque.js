$(document).ready(function () {
    $('#btnProcurar').on("click", () => getProdutoList())
    var ids = $('.id');
})
   


var deleteProduto = function (id) {
    var deletebutton = id.parent().find('.btn-delete');
    var avo = id.parent();
    console.log("aaa")
    deletebutton.on("click", (function () {
        $.ajax({
            type: "POST",
            url: "/Estoque/DeleteProduto",
            data: { id: id.text() }
            ,
            dataType: "json",
            success: function (data) {
                   alert(data.response);
                avo.hide();
            },
            error: function () {
                alert("Não foi possível realizar a deleção");
            }
        })
    }));  
}
function abrirModal(id) {
    const modal = $('#janela-modal')
    if (id != null && id != 0) {
        getProduto(id);
    }
    modal.addClass('abrir')
    modal.on("click", (e) => {
        if (e.target.id == 'fechar' || e.target.id == 'janela-modal') {
            var inputs = modal.find('input');
            inputs.each(function () {
                $(this).val("");
            })
            modal.removeClass('abrir');
        }
    })

}

function getProduto(produtoId) {
    $.ajax({
        type: "Get",
        dataType: "json",
        url: "/Estoque/getProduto",
        data: { id: produtoId },
        success: function (data) {
            if (data != null) {
                $('#m-id').val(data.produtoid);
                $('#m-produto').val(data.descricao);
                $('#m-preco').val(data.preco);
                $('#m-quantidade').val(data.quantidadeemestoque)
            }
        }
    })
}

function getProdutoList() {
    var filter = $('#textoProcurar').val()
    window.location.href = '/Estoque/' +'?descricao=' + filter;
       
}
function saveProduto() {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Estoque/SaveProduto/",
        data: { id: $('#m-id').val(), descricao: $('#m-produto').val(), preco: $('#m-preco').val(), quantidade: $('#m-quantidade').val() },
        success: function (data) {
            alert(data.response);
            window.location.reload();
        },
    });
}