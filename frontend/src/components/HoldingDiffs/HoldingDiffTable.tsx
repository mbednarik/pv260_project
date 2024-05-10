import { Box, Table } from "@mantine/core";
import { HoldingDiffTableRow } from "~/components/HoldingDiffs/HoldingDiffTableRow";
import { HoldingDiff } from "~/types/holdingDiff";

type HoldingDiffTableProps = {
	holdingDiffs: HoldingDiff[];
};

export const HoldingDiffTable = ({ holdingDiffs }: HoldingDiffTableProps) => (
	<Box h="100vh" style={{ overflow: "auto" }}>
		<Table>
			<Table.Thead
				data-testid="holding-diff-table-head"
				pos="sticky"
				top={0}
				bg="blue.1"
			>
				<Table.Tr>
					<Table.Th data-testid="holding-diff-table-head-fund-name">
						Fund name
					</Table.Th>
					<Table.Th data-testid="holding-diff-table-head-company-name">
						Company name
					</Table.Th>
					<Table.Th data-testid="holding-diff-table-head-ticker">
						Ticker
					</Table.Th>
					<Table.Th data-testid="holding-diff-table-head-shares">
						# shares
					</Table.Th>
					<Table.Th data-testid="holding-diff-table-head-shares-change">
						Shares change %
					</Table.Th>
					<Table.Th data-testid="holding-diff-table-head-weight-change">
						Weight change
					</Table.Th>
				</Table.Tr>
			</Table.Thead>
			<Table.Tbody data-testid="holding-diff-table-body">
				{holdingDiffs.map(holdingDiff => (
					<HoldingDiffTableRow
						key={holdingDiff.id}
						holdingDiff={holdingDiff}
					/>
				))}
			</Table.Tbody>
		</Table>
	</Box>
);
