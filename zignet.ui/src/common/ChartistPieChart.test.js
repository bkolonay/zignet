import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import ChartistPieChart from './ChartistPieChart';

it('renders with valid data', () => {
  const chartData = { series: [123, 456] };

  const div = document.createElement('div');
  ReactDOM.render(
    <ChartistPieChart chartId={321} chartData={chartData} />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('renders with empty data', () => {
  const div = document.createElement('div');
  ReactDOM.render(
    <ChartistPieChart />,
    div
  );
  ReactDOM.unmountComponentAtNode(div);
});

it('matches the previous snapshot', () => {
  const chartData = { series: [123, 456] };

  const tree = renderer
    .create(<ChartistPieChart chartId={321} chartData={chartData} />)
    .toJSON();
  expect(tree).toMatchSnapshot();
});