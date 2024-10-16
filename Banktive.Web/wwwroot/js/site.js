﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    $('.balanceCBDCXRPLWallet').each(function () {
        var walletId = $(this).data('walletid');
        fetch('/Wallet/GetNativeBalance/' + walletId, {
            method: "GET",
            headers: { "Content-type": "application/json;charset=UTF-8" }
        })
            .then(response => response.json())
            .then(json =>
                $(this).text(json.valueAsXrp)
            )
            .catch(err => console.log(err));
    });
})

async function generateFundAddress() {
    const client = new xrpl.Client("wss://s.altnet.rippletest.net:51233");
    await client.connect()
    const my_wallet = (await client.fundWallet()).wallet;
    if (my_wallet != null) {
        $('#Form_XRPLAddress').val(my_wallet.address);
        $('#Form_XRPLSeed').val(my_wallet.seed);
        $('#createWalletBtn').removeAttr("disabled");
    }
    
    client.disconnect();
}

async function sendDeferredPayment(seedCode, assetCode, amount, destinationAddress) {
    $('.waitingPayment').css('display', 'block');
    $('.approveCheckBtn').prop("disabled", true);
    $('.rejectCheckBtn').prop("disabled", true);
    try {
        const client = new xrpl.Client("wss://s.altnet.rippletest.net:51233/");
        await client.connect()
        if (seedCode != null) {
            const originWallet = xrpl.Wallet.fromSeed(seedCode);
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'none');
            $('.successPayment').css('display', 'none');

            const prepared = await client.autofill({
                "TransactionType": "CheckCreate",
                "Account": originWallet.address,
                "SendMax": assetCode == 'XRP' ? xrpl.xrpToDrops(amount) : {
                    "currency": assetCode,
                    "value": amount,
                    "issuer": destinationAddress
                },
                "Destination": destinationAddress
            });
            const signed = originWallet.sign(prepared);
            const tx = await client.submitAndWait(signed.tx_blob);
            if (tx.result != null && tx.result.meta != null && tx.result.meta.TransactionResult != null
                && tx.result.meta.TransactionResult == "tesSUCCESS") {
                var checkCreated = tx.result.meta.AffectedNodes.filter(x => x.CreatedNode != null && x.CreatedNode.LedgerEntryType == 'Check');
                if (checkCreated.length == 1) {
                    $('#successfulPayment').val(true);
                    $('#Form_CheckXRPLId').val(checkCreated[0].CreatedNode.LedgerIndex);
                    $('.errorPayment').css('display', 'none');
                    $('.errorWallet').css('display', 'none');
                    $('.successPayment').css('display', 'block');
                    $('.waitingPayment').css('display', 'none');
                    $('.approveCheckBtn').prop("disabled", false);
                    $('.rejectCheckBtn').prop("disabled", false);

                    $('#deferredPaymentForm').submit();
                }
                else {
                    $('#successfulPayment').val(false);
                    $('.errorPayment').css('display', 'block');
                    $('.errorWallet').css('display', 'none');
                    $('.successPayment').css('display', 'none');
                    $('.waitingPayment').css('display', 'none');
                    $('.approveCheckBtn').prop("disabled", false);
                    $('.rejectCheckBtn').prop("disabled", false);
                }
            }
            else {
                $('#successfulPayment').val(false);
                $('.errorPayment').css('display', 'block');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'none');
                $('.waitingPayment').css('display', 'none');
                $('.approveCheckBtn').prop("disabled", false);
                $('.rejectCheckBtn').prop("disabled", false);
            }
            client.disconnect();
        }
        else {
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'block');
            $('.successPayment').css('display', 'none');
            $('.waitingPayment').css('display', 'none');
            $('.approveCheckBtn').prop("disabled", false);
            $('.rejectCheckBtn').prop("disabled", false);
        }

    }
    catch (err) {
        $('.errorPayment').css('display', 'block');
        $('.errorWallet').css('display', 'none');
        $('.successPayment').css('display', 'none');
        $('.waitingPayment').css('display', 'none');
        $('.approveCheckBtn').prop("disabled", false);
        $('.rejectCheckBtn').prop("disabled", false);
    }
}



async function cashMoneyFromPayment(originAddress, seed, assetCode, amount, checkId) {
    $('.errorPayment').css('display', 'none');
    $('.errorWallet').css('display', 'none');
    $('.successPayment').css('display', 'none');
    $('.waitingPayment').css('display', 'block');
    $('.btnRedeem').prop("disabled", true);
    try {
        const originWallet = xrpl.Wallet.fromSeed(seed);
        if (originWallet == null || originWallet.address != originAddress) {
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'block');
            $('.successPayment').css('display', 'none');
            $('.waitingPayment').css('display', 'none');
            $('.btnRedeem').prop("disabled", false);
        }
        else {
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'none');
            $('.successPayment').css('display', 'none');
            $('.waitingPayment').css('display', 'block');
            $('.btnRedeem').prop("disabled", false);
            const client = new xrpl.Client("wss://s.altnet.rippletest.net:51233/");
            await client.connect()
            const prepared = await client.autofill({
                "TransactionType": "CheckCash",
                "Account": originWallet.address,
                "Amount": assetCode == 'XRP' ? xrpl.xrpToDrops(amount) : {
                    "currency": assetCode,
                    "value": amount,
                    "issuer": originWallet.address
                },
                "CheckID": checkId
            });
            const signed = originWallet.sign(prepared);
            const tx = await client.submitAndWait(signed.tx_blob);
            if (tx.result != null && tx.result.meta != null && tx.result.meta.TransactionResult != null
                && tx.result.meta.TransactionResult == "tesSUCCESS" && tx.result.validated == true) {
                $('#successfulPayment').val(true);
                $('.errorPayment').css('display', 'none');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'block');
                $('.waitingPayment').css('display', 'none');
                //$('.btnRedeem').prop("disabled", false);
                $('#cashPaymentForm').submit();
            }
            else {
                $('#successfulPayment').val(false);
                $('.errorPayment').css('display', 'block');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'none');
                $('.waitingPayment').css('display', 'none');
                $('.btnRedeem').prop("disabled", false);
            }


            client.disconnect();
        }
    }
    catch (err) {
        $('.errorPayment').css('display', 'block');
        $('.errorWallet').css('display', 'none');
        $('.successPayment').css('display', 'none');
        $('.waitingPayment').css('display', 'none');
        $('.btnRedeem').prop("disabled", false);
        console.error(err);
    }
}


async function createEscrow(seedCode, amount, destinationAddress, dateFinish) {
    $('.errorPayment').css('display', 'none');
    $('.errorWallet').css('display', 'none');
    $('.successPayment').css('display', 'none');
    $('.waitingPayment').css('display', 'block');
    $('.approveEscrowBtn').prop("disabled", true);
    $('.rejectEscrowBtn').prop("disabled", true);
    try {
        const client = new xrpl.Client("wss://s.altnet.rippletest.net:51233/");
        await client.connect()
        if (seedCode != null) {
            const originWallet = xrpl.Wallet.fromSeed(seedCode);

            const release_date_ripple = dateFinish - 946684800;

            const prepared = await client.autofill({
                "TransactionType": "EscrowCreate",
                "Account": originWallet.address,
                "Amount": xrpl.xrpToDrops(amount),
                "Destination": destinationAddress,
                "FinishAfter": release_date_ripple
            });
            const signed = originWallet.sign(prepared);
            const tx = await client.submitAndWait(signed.tx_blob);
            if (tx.result != null && tx.result.hash != null) {
                $('#successfulPayment').val(true);
                $('#checkId').val(tx.result.hash);
                $('#ledgerSequence').val(tx.result.Sequence);
                $('.errorPayment').css('display', 'none');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'block');
                $('.waitingPayment').css('display', 'none');
                $('.approveEscrowBtn').prop("disabled", false);
                $('.rejectEscrowBtn').prop("disabled", false);
                $('#deferredPaymentForm').submit();
            }
            else {
                $('#successfulPayment').val(false);
                $('.errorPayment').css('display', 'block');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'none');
                $('.waitingPayment').css('display', 'none');
                $('.approveEscrowBtn').prop("disabled", false);
                $('.rejectEscrowBtn').prop("disabled", false);
            }
            client.disconnect();
        }
        else {
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'block');
            $('.successPayment').css('display', 'none');
            $('.waitingPayment').css('display', 'none');
            $('.approveEscrowBtn').prop("disabled", false);
            $('.rejectEscrowBtn').prop("disabled", false);
        }

    }
    catch (err) {
        $('.errorPayment').css('display', 'block');
        $('.errorWallet').css('display', 'none');
        $('.successPayment').css('display', 'none');
        $('.waitingPayment').css('display', 'none');
        $('.approveEscrowBtn').prop("disabled", false);
        $('.rejectEscrowBtn').prop("disabled", false);
    }
}

async function cashMoneyFromEscrow(originAddress, seed, ownerEscrow, offerSequence) {
    $('.errorPayment').css('display', 'none');
    $('.errorWallet').css('display', 'none');
    $('.successPayment').css('display', 'none');
    $('.waitingPayment').css('display', 'block');
    $('.btnRedeem').prop("disabled", true);
    try {
        const originWallet = xrpl.Wallet.fromSeed(seed);
        if (originWallet == null || originWallet.address != originAddress) {
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'block');
            $('.successPayment').css('display', 'none');
            $('.waitingPayment').css('display', 'none');
            $('.btnRedeem').prop("disabled", false);
        }
        else {
            $('.errorPayment').css('display', 'none');
            $('.errorWallet').css('display', 'none');
            $('.successPayment').css('display', 'none');
            $('.waitingPayment').css('display', 'block');
            $('.btnRedeem').prop("disabled", false);
            const client = new xrpl.Client("wss://s.altnet.rippletest.net:51233/");
            await client.connect()
            const prepared = await client.autofill({
                "TransactionType": "EscrowFinish",
                "Account": originWallet.address,
                "Owner": ownerEscrow,
                "OfferSequence": offerSequence
            });
            const signed = originWallet.sign(prepared);
            const tx = await client.submitAndWait(signed.tx_blob);
            console.log(tx);
            if (tx.result != null && tx.result.meta != null && tx.result.meta.TransactionResult != null
                && tx.result.meta.TransactionResult == "tesSUCCESS" && tx.result.validated == true) {
                $('#successfulPayment').val(true);
                $('.errorPayment').css('display', 'none');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'block');
                $('.waitingPayment').css('display', 'none');
                //$('.btnRedeem').prop("disabled", false);
                $('#cashPaymentForm').submit();
            }
            else {
                $('#successfulPayment').val(false);
                $('.errorPayment').css('display', 'block');
                $('.errorWallet').css('display', 'none');
                $('.successPayment').css('display', 'none');
                $('.waitingPayment').css('display', 'none');
                $('.btnRedeem').prop("disabled", false);
            }


            client.disconnect();
        }
    }
    catch (err) {
        $('.errorPayment').css('display', 'block');
        $('.errorWallet').css('display', 'none');
        $('.successPayment').css('display', 'none');
        $('.waitingPayment').css('display', 'none');
        $('.btnRedeem').prop("disabled", false);
        console.error(err);
    }
}