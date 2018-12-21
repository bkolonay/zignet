import React, { Component } from 'react';
import TestResultRow from './TestResultRow'
import HomeLink from '../common/HomeLink'
import ChartButton from './ChartButton'
import Filter from '../home/Filter'

class LatestTestResultsList extends Component {

  render() {
    return (
      <div className="container">
        <h4><HomeLink filter={this.props.filter} /></h4>
        <div className="row">
          <div className="col-4"/>
          <div className="col-4">
            <h2 className="text-center">{this.props.suiteName}</h2>
          </div>
          <div className="col-4">
            <ChartButton filter={this.props.filter} />
          </div>
        <div className="row">
          <Filter filter={this.props.filter} onFilterChange={this.props.onFilterChange} />
        </div>          
        </div>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Name</th>
              {null && <th scope="col">Type</th>}
              <th scope="col">Status</th>
              <th scope="col">History</th>
            </tr>
          </thead>
          <tbody>
            {this.props.testResults.map((testResult) =>
              <TestResultRow key={testResult.TestResultID} testResult={testResult} />
            )}
          </tbody>
        </table>
      </div>
    );
  }
}

export default LatestTestResultsList;
