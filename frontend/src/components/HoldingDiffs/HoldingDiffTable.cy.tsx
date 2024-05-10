import { HoldingDiffTable } from "~/components/HoldingDiffs/HoldingDiffTable";
import { HoldingDiff } from "~/types/holdingDiff";

const HOLDING_DIFFS_MANY_ROWS: HoldingDiff[] = [
	{
		id: 1,
		fundName: "Fund 1",
		companyName: "Company 1",
		ticker: "TICKER1",
		oldShares: 100,
		sharesChange: 10,
		oldWeight: 10,
		weightChange: 0.1,
	},
	{
		id: 2,
		fundName: "Fund 2",
		companyName: "Company 2",
		ticker: "TICKER2",
		oldShares: 200,
		sharesChange: 20,
		oldWeight: 20,
		weightChange: 0.2,
	},
	{
		id: 3,
		fundName: "Fund 3",
		companyName: "Company 3",
		ticker: "TICKER3",
		oldShares: 300,
		sharesChange: 30,
		oldWeight: 30,
		weightChange: -0.3,
	},
	{
		id: 4,
		fundName: "Fund 4",
		companyName: "Company 4",
		ticker: "TICKER4",
		oldShares: 400,
		sharesChange: 40,
		oldWeight: 10,
		weightChange: 0.4,
	},
	{
		id: 5,
		fundName: "Fund 5",
		companyName: "Company 5",
		ticker: "TICKER5",
		oldShares: 500,
		sharesChange: -10,
		oldWeight: 5,
		weightChange: 0.5,
	},
];

const HOLDING_DIFF_POSITIVE: HoldingDiff[] = [
	{
		id: 1,
		fundName: "Fund 1",
		companyName: "Company 1",
		ticker: "TICKER1",
		oldShares: 100,
		sharesChange: 10,
		oldWeight: 10,
		weightChange: 0.1,
	},
];

const HOLDING_DIFF_NEGATIVE: HoldingDiff[] = [
	{
		id: 1,
		fundName: "Fund 1",
		companyName: "Company 1",
		ticker: "TICKER1",
		oldShares: 100,
		sharesChange: -20,
		oldWeight: 10,
		weightChange: -0.2,
	},
];

describe("<HoldingDiffTable />", () => {
	it("renders", () => {
		cy.mount(<HoldingDiffTable holdingDiffs={[]} />);

		cy.get("[data-testid=holding-diff-table-head]").should("exist");
		cy.get("[data-testid=holding-diff-table-body]").should("exist");
	});
});

describe("<HoldingDiffTable />", () => {
	it("displays correct header columns", () => {
		cy.mount(<HoldingDiffTable holdingDiffs={[]} />);

		cy.get("[data-testid=holding-diff-table-head]").should("exist");
		cy.get("[data-testid^=holding-diff-table-head-]").should(
			"have.length",
			6
		);

		cy.get("[data-testid=holding-diff-table-head-fund-name]").should(
			"have.text",
			"Fund name"
		);
		cy.get("[data-testid=holding-diff-table-head-company-name]").should(
			"have.text",
			"Company name"
		);
		cy.get("[data-testid=holding-diff-table-head-ticker]").should(
			"have.text",
			"Ticker"
		);
		cy.get("[data-testid=holding-diff-table-head-shares]").should(
			"have.text",
			"# shares"
		);
		cy.get("[data-testid=holding-diff-table-head-shares-change]").should(
			"have.text",
			"Shares change %"
		);
		cy.get("[data-testid=holding-diff-table-head-weight-change]").should(
			"have.text",
			"Weight change"
		);
	});
});

describe("<HoldingDiffTable />", () => {
	it("displays correct number of rows", () => {
		cy.mount(<HoldingDiffTable holdingDiffs={HOLDING_DIFFS_MANY_ROWS} />);

		cy.get("[data-testid=holding-diff-table-body]").should("exist");
		cy.get(
			"[data-testid^=holding-diff-table-row][data-testid$=-wrapper]"
		).should("have.length", HOLDING_DIFFS_MANY_ROWS.length);
	});
});

describe("<HoldingDiffTable />", () => {
	it("correctly displays positive change", () => {
		cy.mount(<HoldingDiffTable holdingDiffs={HOLDING_DIFF_POSITIVE} />);

		cy.get("[data-testid=holding-diff-table-body]").should("exist");
		cy.get(
			"[data-testid^=holding-diff-table-row][data-testid$=-wrapper]"
		).should("have.length", HOLDING_DIFF_POSITIVE.length);

		cy.get(
			`[data-testid=holding-diff-table-row-${HOLDING_DIFF_POSITIVE[0].id}-shares-change]`
		).should("contain.text", "10%");

		cy.get(
			`[data-testid=holding-diff-table-row-${HOLDING_DIFF_POSITIVE[0].id}-weight-change]`
		).should("contain.text", "0.1");
	});
});

describe("<HoldingDiffTable />", () => {
	it("correctly displays negative change", () => {
		cy.mount(<HoldingDiffTable holdingDiffs={HOLDING_DIFF_NEGATIVE} />);

		cy.get("[data-testid=holding-diff-table-body]").should("exist");
		cy.get(
			"[data-testid^=holding-diff-table-row][data-testid$=-wrapper]"
		).should("have.length", HOLDING_DIFF_POSITIVE.length);

		cy.get(
			`[data-testid=holding-diff-table-row-${HOLDING_DIFF_NEGATIVE[0].id}-shares-change]`
		).should("contain.text", "-20%");

		cy.get(
			`[data-testid=holding-diff-table-row-${HOLDING_DIFF_NEGATIVE[0].id}-weight-change]`
		).should("contain.text", "0.2");
	});
});
