USE [GuichetAutomatique]
GO
/****** Object:  StoredProcedure [dbo].[sp_Paiement]    Script Date: 2023-08-17 16:26:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Paiement]
    @IdFacture INT,
    @IdCompteProv INT,
    @IdUtilisateur INT,
    @MontantTransaction DECIMAL(10, 2)
AS
BEGIN
    BEGIN TRY
        DECLARE @TypeCompteProv NVARCHAR(50)
        DECLARE @SoldeProv DECIMAL(10, 2)
        DECLARE @MontantRestant DECIMAL(10, 2)
        
		-- Récupérer les informations du compte de provenance sélectionné et de la facture liée au client.
        SELECT @TypeCompteProv = TypeCompte, @SoldeProv = Solde, @MontantRestant = SoldeFacture FROM Comptes c
        JOIN Factures f ON c.IdUtilisateur = f.IdUtilisateur
        WHERE c.IdCompte = @IdCompteProv AND c.IdUtilisateur = @IdUtilisateur AND f.IdFacture = @IdFacture;

        -- Procéder à la transaction.
        BEGIN TRANSACTION;

        -- Si le compte de paiement est un compte chèque et que le montant de la facture ainsi que le frais est plus bas
		-- que le solde du compte en provenance.
        IF @TypeCompteProv = 'Chèque'
        BEGIN
            IF @MontantTransaction + 1.25 <= @SoldeProv
            BEGIN
                -- déduction des fonds du compte en provenance.
                UPDATE Comptes
                SET Solde = Solde - (@MontantTransaction + 1.25)
                WHERE IdCompte = @IdCompteProv;

                -- Insertion de la transaction dans la table transactions.
                INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
                VALUES ('Paiement de facture', @IdCompteProv, -@MontantTransaction, GETDATE());

                -- Mise à jour du montant restant dans la table facture si celle-ci est partiellement réglée.
                UPDATE Factures
                SET SoldeFacture = SoldeFacture - @MontantTransaction
                WHERE IdFacture = @IdFacture;

                -- Si la facture est réglée en totalité.
                IF @MontantRestant - @MontantTransaction = 0
                BEGIN
                    UPDATE Factures
                    SET Statut = 'Payée'
                    WHERE IdFacture = @IdFacture;
                END

                -- Insertion des frais dans la table transactions.
                INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
                VALUES ('Frais de facturation', @IdCompteProv, -1.25, GETDATE());

                -- Commit the transaction
                COMMIT TRANSACTION;
            END
            ELSE
            BEGIN
                THROW 50004, 'Impossible d''effectuer le paiement de la facture puisque le solde du compte est insuffisant.', 1;
            END
        END
        ELSE
        BEGIN
            THROW 50003, 'Le compte de provenance doit être un compte chèque.', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END