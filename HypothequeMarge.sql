CREATE PROCEDURE HypthequeMarge
    @hypothequeId INT,
    @montant DECIMAL
AS
BEGIN
    DECLARE @hypothequeSolde DECIMAL
    DECLARE @margeDeCreditId INT
    DECLARE @margeDeCreditSolde DECIMAL

    -- Récupérer le solde de l'hypothèque.
    SELECT @hypothequeSolde = Solde
    FROM Comptes
    WHERE IdCompte = @hypothequeId

    -- Vérification de la balance de l'hypothèque.
    IF @hypothequeSolde >= @montant
    BEGIN
        -- Retrait du compte hypothèque.
        UPDATE Comptes SET Solde = Solde - @montant WHERE IdCompte = @hypothequeId
        -- Retrait avec succès
		RETURN 0; 
    END
    ELSE
    BEGIN
		-- Calcul du montant restant du retrait à partir de la marge de crédit.
        DECLARE @retraitMarge DECIMAL
        SET @retraitMarge = @montant - @hypothequeSolde
		
		-- Associer la marge au bon client.
        SELECT @margeDeCreditId = IdCompte, @margeDeCreditSolde = Solde
        FROM Comptes
        WHERE TypeCompte = 'Marge de crédit' AND IdUtilisateur = (SELECT IdUtilisateur FROM Comptes WHERE IdCompte = @hypothequeId)

        -- Vérifier si le client à une marge.
        IF @margeDeCreditId IS NULL
        BEGIN
            -- Si le client n'a pas de marge et le montant du retrait excède le solde du compte.
            RETURN 2; 
        END
        ELSE
        BEGIN
			-- Vérifier si la limite de crédit est suffisante pour effectuer le retrait.
            IF @margeDeCreditSolde >= @retraitMarge
            BEGIN
                UPDATE Comptes SET Solde = Solde - @hypothequeSolde WHERE IdCompte = @hypothequeId
                UPDATE Comptes SET Solde = Solde - @retraitMarge WHERE IdCompte = @margeDeCreditId
                RETURN 0; -- Successful withdrawal
            END
            ELSE
            BEGIN
				-- Balance insuffisante.
                RETURN 1;
            END
        END
    END
END
