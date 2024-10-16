# Banktive


**[A full description (5 minutes) is enabled in this video](https://youtu.be/DowjxGYj-68)**

## Problem
The advancement of Web 3.0 and CBDC technologies is going at a fast pace and it is increasingly clear that they have a future (immediate payments, low cost, inclusive, and more), but what has not advanced so fast the the growth of non-technical users in this type of applications, either due to ignorance or technical barriers to entry.

Every day it becomes more necessary to create apps that use Blockchain, but regardless of whether users know about these technologies or not, in the same way that people use bank accounts and credit cards without knowing the systems behind them (for example, the SWIFT system).

That is why it is necessary to take advantage of XRPL so that money can flow between people in a simple way, and this is achieved with a Neobank, where thanks to this Blockchain technology can use any type of money (fiat, crypto, and CBDC) in a simple way like any other simple platform.

## Banktive
**Banktive** is a neobank built entirely on the XRPL network, taking advantage of its capabilities to be a bank for everyone (web3 newbies or experienced) to be able to send and receive money, pay bills, and much more like any other financial platform.

With Banktive, anyone can manage their accounts transparently, without the need for technical knowledge about Web3, and do the same things they do with their current banks, with the difference that Banktive is for everyone, regardless of economic level, level of bankarization, country, etc.

## What it does
The current version (1.0) of Banktive (using the XRPL testnet) allows:

- **Manage profile**: You can enter certain personal data, such as address and Tax ID (which will soon be used for a KYC process, like any other bank). You can also manage your account (password, etc.).
- **Manage wallets**: You can create, update, and monitor wallet balances on the XRPL network, including each wallet with a remember/share alias, instead of complex XRPL addresses. At the moment, the currency for all the wallets is only XRP.
- **Destinations**: You can manage the recipients of your payments (create, update, view, and delete), whether you know their XRPL address, alias, or ID (soon you will be able to add other banks).
- **Payments**: You can send and receive payments simply and immediately. For this version, you can only send XRP (using Payments in XRPL).
- **Transfers**: Monitor payments sent and received, see every detail, and filter them.
- **Deposits**: Issue payments in deposits to send the money to the network so that the recipient can cash it at a future date (using Escrows in XRPL).
- **Credit**: Make credit payments (as you would with a credit card) to a recipient who can debit it from your wallet on a specific date (using Checks in XRPL).
- **Services**: If you are a service provider (professional, water, electricity, etc.) you can manage your customer accounts (add accounts, collections with expiration dates, see all payments), and if you are a subscriber, you can subscribe to your accounts and pay them without knowing the recipient of the payment (such as when paying for a service through your current bank).

## Second phase

The new version for Banktive (2.0) for the second phase , will have the following aspects (along with those mentioned above):

- **Wallets**: When creating the wallet, the user will be able to select the main currency for that wallet (not only XRP).
- **Subwallets**: This new module will allow the creation of subwallets, where the user can control aspects such as balance or a maximum number of payment amounts (for example, so that parents can control the money of their minor children).
- **Payments with other currencies**: In both the payment module and the credit module, you can define the currency with which you will pay (independent of the main currency of the wallet selected for payment).
- **Streaming payments**: This new module will allow you to start a payment and constantly send payments using a payment channel, which the creator and/or recipient can end to execute the final payment.
- **Assets**: In this new module, users will be able to buy different assets available on the network through the decentralized exchange.
- **Improvements in design aspects**.
- **Integration with Private Ledger**.

## Technologies
Banktive is built with:

- **Microsoft .NET Core Framework**: For the backend and a large part of the front end. Using C# for much of the interaction with XRPL (using external libraries and RPC calls). Use the [XrplCSharp library](https://github.com/Transia-RnD/XrplCSharp).
- **HTML, CSS, and Javascript**: For the front end and some aspects of XRPL that cannot be achieved in the backend.
- **SQL Server**: For storage of data that does not go to the XRPL network, and also some details of each transaction in XRPL.
- **XRPL**: For the interaction in Blockchain, for now in testnet.

## XRPL
Using XRPL, this network is and will be fundamental to the new financial system, and a noebank is part of it, because we believe that the future must be the intersection of the best of fiat, what best of crypto, and best of CBDC, and XRPL enables it.

In particular, **Banktive** uses the following attributes of the XRPL network:

- **Accounts**: For the creation and management of wallets on the network.
- **Payments**: For "normal" payments.
- **Checks**: For payments with credit through wallets, and the platform controls when the recipient can cash it.
- **Escrows**: For payments with deposit (send money to an escrow) so that the recipient can cash it on a certain date.
- **Payment Channel**: For streaming payments to a particular recipient.
- **Trust lines**: For cross-currency payments.
- **Multi-sign wallets**: For enabling sub-wallets. 
- **Cross-Currency**: For payments with currencies other than XRP, both in normal payments and checks.
- **DEX**: To buy coins with the available balance in the wallets (using Offers/Auto-bridging).

## What's next for Banktive
We are working to make Banktive a reality and become the first neobank in the world built entirely on Web3 thanks to XRPL.

To do this, our next steps will be:
- Raise capital.
- Improve architecture and technical aspects.
- Start alliances (with Ripple and other organizations).
- Obtain clients.
