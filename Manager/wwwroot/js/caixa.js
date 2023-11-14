var total = 0;
function finalizarCompra() {
    var itens = [];
    $('.modal-itens .item').each(function () {
        var id = $(this).find('.item-id').text();
        var quantidade = $(this).find('.item-quantity').text();
        console.log(id);
        console.log(quantidade);
        var item = { Id: id, Quantidade: quantidade }
        itens.push(item);
    })
    var parcelado = $('#Pagamento').val() == 1;
    var cpf = $('.cpf').val();
    var valorPago = $('#valor-pago').val();
    console.log("total= " + total);
    console.log("pago = " + valorPago);
    if (!parcelado && valorPago != total) {
        alert("Em caso de pagamento a vista é necessário o pagamento integral");
    }
    else if (parcelado && cpf.trim() == "") {
        alert("Em caso de parcelamento é necessário informar o cpf");
    }
    else {
        $.ajax({
            url: "Caixa/FinalizarCompra",
            type: "POST",
            data: { itens: itens, cpf: cpf, valorPago: valorPago, parcelado: parcelado},
            datatype: "Json",
            success: function (data) {
                alert(data.result);
                if (data.success) {
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                }
            }
        });
    }
}
function cadastraCliente() {
    console.log($('#nome').val());
    console.log($('#nasc').val());
           
    $.ajax({
        url: "/Caixa/CadastrarCliente",
        type: "POST",
        datatype: "Json",
        data: {
            nomeCompleto: $('#nome').val(),
            dataNascimento: $('#nasc').val(),
            rg: $('#rg').val(),
            cpf: $("#cpf").val(),
            endereco: $("#endereco").val(),
            cidade: $("#cidade").val(),
            cep: $("#cep").val(),
            contato: $("#contato").val()
        },
        success: function (data) {
            alert(data.result);
            if (data.success) {
                $('#cadastra-Cliente')[0].reset()

            }
        }
    })
}
function abrirModal(modalPraAbrir) {
    const modal = $('.janela-modal');
    modal.each(function(){
        $(this).removeClass('abrir')
    })
    modal.each(function () {
        $(this).on('click', (e) => {
            if (e.target.id == 'fechar') {
                $(this).removeClass('abrir');
                if (modalPraAbrir == "janela-modal") {
                    $('#janela-modal .item').remove();
                }
            }
        })
    })
    if (modalPraAbrir == "janela-modal") {
        var itemVazio = false;
        
        $('.itens').each(function(){
            var quantidade = $(this).find('.item-quantity').children().val();
            if (quantidade <= 0 || quantidade == undefined) {
                itemVazio = true;
            }
        })
        if (itemVazio) {
            alert("Há itens com valores invalidos. Remova-os ou preencha os valores corretamente.");
        }
        else {
            var itens = $('.itens').html();
            $('.modal-itens').append(itens);
            $('.modal-itens .btn-delete').remove();
            $('.modal-itens  input').remove();
            $('.modal-itens td').each(function () {
                $(this).removeClass('numero');
            })
            var quantidades = $('.modal-itens .item-quantity');
            var quantidadesTela = $('.itens .item-quantity');
            for (i = 0; i < quantidadesTela.length; i++) {
                var elementoTela = quantidadesTela[i];
                var elemento = quantidades[i];
                $(elemento).text(($(elementoTela).children().val()));
            }

            somaTotal();

            $('#janela-modal').addClass('abrir');
        }
    }
    else {

        $('#segunda-modal').addClass('abrir');
    }
    
}

$(document).ready(function(){
    preencheSeletor();
    $('.btn-search').on("click", () => getProduto());
    $('.itens').on("click", ".btn-delete",(event) => removeProduto(event))
})

function preencheSeletor(){
    $.ajax({
        type: "GET",
        url:"/Estoque/GetProdutosPraCaixa",
        datatype: "json",
        success: function(produtos){
            for(i = 0; i < produtos.length; i++){
                var produtoId = produtos[i].id;
                var nome = produtos[i].nome;
                $('.search-box').append("<option value=\"" + produtoId + "\">" + nome + "</option>");
            }
        }
    })
}

function getProduto() {
    var id = $('#search-box').val();
    if (id != 0) {
        $.ajax({
            url: "/Estoque/GetProduto",
            type: "GET",
            datatype: "json",
            data: { id: id },
            success: function (data) {
                $('.itens').append("<tr class=\"item\"> <td class=\"item-id\">" + data.produtoid + "</td>" +
                    "<td class=\"item-name\">" + data.descricao + "</td>" + "<td class=\"item-price\">" +
                    "R$" + data.preco + "<td class=\"item-quantity\"><input class=\"numero\" placeholder=\"Quantidade\"></td>" +
                    "<td class=\"btn-delete\"><button type =\"button\" class=\"table-btn delete-btn\"> Excluir</button ></td >"
                    + "</tr>"
                );
                $('.itens .numero').mask('0#');
            }
        })
    }
    else {
        alert("Selecione um produto");
    }
}
function removeProduto(e) {
    var $button = $(e.currentTarget);
    $button.parent().remove();
}
function somaTotal() {
    total = 0;
    var itens = $('.modal-itens .item');
    itens.each(function () {
        var quantidade = $(this).find('.item-quantity').text();
        var preco = $(this).find('.item-price').text().substring(2);
        var valorTotalItem = parseFloat(quantidade) * parseFloat(preco);
        total = total + valorTotalItem;
    })
    $('#valor-total').text("Total da Compra:R$" + total.toFixed(2));
}