import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import ChartistPieChart from '../ChartistPieChart';

it('renders with valid data', () => {
  const component = <ChartistPieChart chartId={1} chartData={{ series: [123, 456] }} />
  ReactDOM.render(component, document.createElement('div'));
});

it('renders with empty data', () => {
  const component = <ChartistPieChart />
  ReactDOM.render(component, document.createElement('div'));
});

it('snapshot with valid data', () => {
  const component = <ChartistPieChart chartId={123} chartData={{ series: [123, 456] }} />
  const tree = renderer.create(component).toJSON();  
  expect(tree).toMatchSnapshot();
});

it('snapshot with no data', () => {
  const component = <ChartistPieChart />
  const tree = renderer.create(component).toJSON();  
  expect(tree).toMatchSnapshot();
});