export type HoldingDiffsQueryParams = {
	fundId: string;
	oldHoldingDate: Date;
	newHoldingDate: Date;
};

export type HoldingDiff = {
	id: number;
	oldShares: number;
	sharesChange: number;
	oldWeight: number;
	weightChange: number;
	fundName: string;
	companyName: string;
	ticker?: string;
};
