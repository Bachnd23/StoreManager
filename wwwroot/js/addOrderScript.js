$(document).ready(function () {
    // Add row functionality
    $('#addRow').click(function () {
        var newRowHtml = `
                    <div class="row bg-light rounded h-100 p-4 align-items-center added-row">
                        <div class="col-md-4">
                            <div class="mb-3">
                                <label for="selectCustomer" class="form-label">Chọn khách hàng</label>
                                <select class="form-select" name="customerId" id="selectCustomer">
                                    <!-- Populate options dynamically if needed -->
                                </select>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="mb-3">
                                <label for="selectItem" class="form-label">Chọn mặt hàng</label>
                                <select class="form-select" name="itemId" id="selectItem">
                                    <!-- Populate options dynamically if needed -->
                                </select>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="mb-3">
                                <label for="inputQuantity" class="form-label">Số lượng</label>
                                <input type="number" name="quantity" class="form-control" id="inputQuantity" value="">
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="mb-3">
                                <label for="inputOrderDate" class="form-label">Ngày đặt</label>
                                <input type="date" name="orderDate" class="form-control" id="inputOrderDate" value="">
                            </div>
                        </div>
                    </div>
                `;

        $('#rowsContainer').append(newRowHtml);
    });
});