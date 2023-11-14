$(document).ready(function () {
    $('.modal-btn').on("click", () => pagarDividas())
})
function abrirModal(id) {
    const modal = $('.janela-modal');
    modal.addClass('abrir')

    modal.on('click', (e) => {
        if (e.target.id == 'fechar' || e.target.id == 'janela-modal') {
            var inputs = modal.find('input');
            inputs.each(function () {
                $(this).val("");
            })
            modal.removeClass('abrir')
        }
    })
    if (id != null) {
        getCaloteiro(id)
    }
}

function getCaloteiro(id) {
    $.ajax({
        url: "/Contas/GetDividaCliente",
        type: "Get",
        datatype: "Json",
        data: { Clienteid: id }
            ,
        success: function (data) {
            $('#clienteid').val(data.clienteid);
            $('#cliente').val(data.nome);
            $('#valorPendente').val(data.valorpendente);
        }

    })
}
function pagarDividas() {
    var clienteid = $('#clienteid').val();
    var valorPago = $('#valorPago').val();
    $.ajax({
        url: "Contas/quitarDividas",
        type: "Post",
        dataype: "Json",
        data: {clienteId : clienteid, valorPago: valorPago},
        success: function (data) {
            alert(data.content);
            if (data.success) {
                setTimeout(function () {
                    location.reload();
                }, 2000);
            }
        }
    })
}