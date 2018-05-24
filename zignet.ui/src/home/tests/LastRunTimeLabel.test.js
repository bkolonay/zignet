import React from 'react';
import ReactDOM from 'react-dom';
import renderer from 'react-test-renderer';
import LastRunTimeLabel from '../LastRunTimeLabel';
import { getTimeFromNowWithSuffix } from '../../common/UtcDateProvider';

jest.mock('../../common/UtcDateProvider');

it('renders when not grouped', () => {
  const component = <LastRunTimeLabel grouped={false} suiteEndTime={'2018-05-24T01:00:00'} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('renders when grouped', () => {
  const component = <LastRunTimeLabel grouped={true} suiteEndTime={'2018-05-24T01:00:00'} />;
  ReactDOM.render(component, document.createElement('div'));
});

it('snapshot when not grouped and has end time', () => {
  getTimeFromNowWithSuffix.mockReturnValue('3 days ago');
  const component = <LastRunTimeLabel grouped={false} suiteEndTime={'2018-05-24T01:00:00'} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped', () => {
  const component = <LastRunTimeLabel grouped={true} suiteEndTime={'2018-05-24T01:00:00'} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when grouped and no suite end time', () => {
  const component = <LastRunTimeLabel grouped={true} suiteEndTime={undefined} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});

it('snapshot when not grouped and no suite end time', () => {
  const component = <LastRunTimeLabel grouped={false} suiteEndTime={undefined} />;
  const tree = renderer.create(component).toJSON();
  expect(tree).toMatchSnapshot();
});