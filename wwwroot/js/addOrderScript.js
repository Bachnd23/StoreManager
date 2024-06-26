$(document).ready(function () {
    // Initialize chosen select
    $('.chosen-select').chosen();

    var orderIndex = 1;

    $('#add-order-row').click(function () {
        var newOrderRow = $('.order-row:first').clone();
        newOrderRow.find('input, select').each(function () {
            var name = $(this).attr('name').replace('Orders[0]', 'Orders[' + orderIndex + ']');
            $(this).attr('name', name).val('');
        });
        $('#order-rows').append(newOrderRow);
        $('.chosen-select').chosen('destroy').chosen(); // Reinitialize chosen after cloning
        orderIndex++;
    });

    $(document).on('click', '.remove-order-row', function () {
        if ($('.order-row').length > 1) {
            $(this).closest('.order-row').remove();
        }
    });

    $('#topSelectCustomer').change(function () {
        var selectedCustomer = $(this).val();
        $('select[name^="Orders"][name$=".CustomerId"]').val(selectedCustomer).trigger('chosen:updated');
    });

    $('#topSelectOrderDate').change(function () {
        var selectedDate = $(this).val();
        $('input[name^="Orders"][name$=".Date"]').val(selectedDate);
    });
});